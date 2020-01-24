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
		protected Task(string name, Effect longTermEffect, Effect shortTermEffect, Expression requiredTime) 
		{
			GameManager.TimeElapsed += AdvanceProgress;
			Name = name;
			LongTermEffect = longTermEffect;
			_ShortTermEffect = shortTermEffect;
			_RequiredTime = requiredTime;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public ConfidentialLevel ConfidentialLevel { get; set; }
		public string Name { get; set; }

		protected Expression _RequiredTime;
		private int _TimeElapsed = 0;

		protected Effect _ShortTermEffect;

		protected Effect _LongTermEffect;
		/// <summary>
		///		Use its encapsulated field occasionally to avoid side effect
		/// </summary>
		public Effect LongTermEffect
		{
			get => _LongTermEffect;
			set
			{
				if (value != _LongTermEffect)
				{
					_LongTermEffect = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongTermEffect)));
				}
			}
		}

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
				Misc.Round(_RequiredTime.Value - _TimeElapsed)));
		}

		private void ImposeLongTermEffect()
		{
			for (int i = 0; i < ValueComplexNTuple.NUM_VALUES; i++)
			{
				Data.Me.NationalIndices[i].Growth.AddTerm(Name, LongTermEffect[i]);
			}
		}
	}

	public class Census : Task
	{
		public Census(string name, int valueIndex) : base(name, null, null, null) 
		{
			_Index = valueIndex;
			_ShortTermEffect = new Effect();
			Init();
			ImposeShortTermEffect();
		}

		private int _Index;
		public Action<Task> DoCensus;

		private void Init()
		{
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
					LongTermEffect = new Effect(economy: new Expression(1, p => 1 + p * Data.Me.AdjustedBureaucracyIndex));
					_RequiredTime = new Expression(2, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;

				default:
					LongTermEffect = new Effect();
					_RequiredTime = new Expression(1, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;
			}
		}

		public override void Execute()
		{
			Data.Me.OutdatedNationalIndices[_Index].Update(Data.Me.NationalIndices[_Index]);
		}

		private void ImposeShortTermEffect()
		{
			for (int i = 0; i < ValueComplexNTuple.NUM_VALUES; i++)
			{
				Data.Me.NationalIndices[i].CurrentValue -= _ShortTermEffect[i].Value;
			}
		}
	}

	public class Policy : Task
	{
		private Action _Action;

		public Policy(string name, Effect longTermEffect, Effect shortTermEffect, Expression requiredTime) : 
			base(name, longTermEffect, shortTermEffect, requiredTime) { }
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
		public Manufacture(string name, Func<IDestroyable> instruction, Effect longTermEffect, Effect shortTermEffect, 
			Expression requiredTime) : base(name, longTermEffect, shortTermEffect, requiredTime) 
		{
			_Instruction = instruction;
			LongTermEffect = longTermEffect;
			_ShortTermEffect = shortTermEffect;
		}

		private Func<IDestroyable> _Instruction;

		public IDestroyable FinalProduct { get; private set; } = null;

		public override void Execute()
		{
			FinalProduct = _Instruction();
		}
	}

	public class Transportation : Task
	{
		public Transportation(string name, Effect longTermEffect, Effect shortTermEffect, Expression requiredTime) : 
			base(name, longTermEffect, shortTermEffect, requiredTime) { }
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
		public Deployment(string name, Region destination, IDeployable deployable, Effect longTermEffect, 
			Effect shortTermEffect, Expression requiredTime) : base(name, longTermEffect, shortTermEffect, requiredTime) 
		{
			DeployableObject = deployable;
			Destination = destination;
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
