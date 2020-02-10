using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization.DataStructures
{
	/* Stage is a general category of any action made by the gov't. It can be: 
	 * 1. Investigation and census (get info from sources (in-game entities))
	 * 2. Enactments and policies (make changes to values/in-game entities)
	 * 3. Orders to manufacture (create new in-game entities and deploy) 
	 * etc. */
	public abstract class Task : IExecutable, INotifyPropertyChanged
	{
		protected Task(string name, CostOfStage cost) 
		{
			GameManager.TimeElapsed += AdvanceProgress;
			Name = name;
			Cost = cost;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public ConfidentialLevel ConfidentialLevel { get; set; }
		public string Name { get; set; }

		private int _TimeElapsed = 0;

		public CostOfStage Cost { get; protected set; }

		/// <summary>
		///		Execute the task only if it is finally finished
		/// </summary>
		public abstract void Execute();

		protected void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}

		private void AdvanceProgress()
		{
			_TimeElapsed++;
			ImposeLongTermEffect();
			EventManager.RaiseEvent(this, new TaskProgressAdvancedEventArgs(
				Misc.Round(Cost.RequiredTime.Value - _TimeElapsed)));
		}

		private void ImposeLongTermEffect()
		{
			for (int i = 0; i < ValueComplexNTuple.NUM_VALUES; i++)
			{
				Data.Me.NationalIndices[i].Growth.AddTerm(Name, Cost.LongTermEffect[i]);
			}
		}
	}

	public class Census : Task
	{
		public Census(string name, int valueIndex) : base(name, null) 
		{
			_Index = valueIndex;
			Init();
			ImposeShortTermEffect();
		}

		private int _Index;
		public Action<Task> DoCensus;

		private void Init()
		{
			Effect longTermEffect;
			Expression requiredTime;
			switch (_Index)
			{
				// Population
				case 1:
				// Stability
				case 7:
				// Nationalism
				case 8:
				// Satisfaction
				case 9:
				// Bureaucracy
				case 10:
					longTermEffect = new Effect(economy: new Expression(1, p => 1 + p * Data.Me.AdjustedBureaucracyIndex));
					requiredTime = new Expression(2, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;

				default:
					longTermEffect = new Effect();
					requiredTime = new Expression(1, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;
			}
			Cost = new CostOfStage(
				longTermEffect,
				shortTermEffect: new Effect(),
				requiredTime
			);
		}

		public override void Execute()
		{
			Data.Me.OutdatedNationalIndices[_Index].Update(Data.Me.NationalIndices[_Index]);
		}

		private void ImposeShortTermEffect()
		{
			for (int i = 0; i < ValueComplexNTuple.NUM_VALUES; i++)
			{
				Data.Me.NationalIndices[i].CurrentValue -= Cost.ShortTermEffect[i].Value;
			}
		}
	}

	public class Policy : Task
	{
		private Action _Action;

		public Policy(string name, CostOfStage cost, Action action) : 
			base(name, cost) 
		{
			_Action = action;
		}
		//public static Policy Create(Action action, Effect longTermEffect, Expression requiredTime)
		//{
		//	return new Policy
		//	{
		//		_LongTermEffect = longTermEffect,
		//		_RequiredTime = requiredTime,
		//		_Action = action
		//	};
		//}

		public override void Execute() => _Action();
	}

	public class Manufacture : Task
	{
		// Any object that is manufactured will not be automatically added to anywhere. 
		// Its destination should always be explicitly stated. 
		// e.g. send it to reserve
		public Manufacture(string name, Func<IDestroyable> instruction, CostOfStage cost)
			: base(name, cost) 
		{
			_Instruction = instruction;
			_Instruction = instruction;
		}

		private readonly Func<IDestroyable> _Instruction;

		public IDestroyable FinalProduct { get; private set; } = null;

		public override void Execute()
		{
			FinalProduct = _Instruction();
		}
	}

	public class Transportation : Task
	{
		public Transportation(string name, CostOfStage cost) : 
			base(name, cost) { }
		public Region From { get; set; }
		public Region Next { get; set; }
		public Region To { get; set; }

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}

	public class Deployment : Task
	{
		public Deployment(string name, Region destination, IDeployable deployable, CostOfStage cost) : 
			base(name, cost) 
		{
			DeployableObject = deployable;
			Destination = destination;
			Cost = cost;
			_Execute = () => 
			throw new NotImplementedException();
		}

		private Action _Execute;
		public IDeployable DeployableObject { get; private set; } = null;
		public Region Destination { get; }

		public override void Execute()
		{
			DeployableObject.DeployedRegion = Destination;
		}
	}

	public enum ConfidentialLevel
	{
		/// <summary>
		///		Remain mostly unaware even within the central government
		/// </summary>
		TopSecret = 0,

		/// <summary>
		///		Opaque to the public
		/// </summary>
		Secret = 1,

		/// <summary>
		///		Not intentionally informing other nations
		/// </summary>
		Domestic = 2,

		/// <summary>
		///		Actively advertising
		/// </summary>
		Global = 3
	}
}
