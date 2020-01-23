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
		protected Task() 
		{
			GameManager.TimeElapsed += AdvanceProgress;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ConfidentialLevel ConfidentialLevel { get; set; }

		protected Expression _RequiredTime;
		private int _TimeElapsed = 0;

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

		protected void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}

		private void AdvanceProgress()
		{
			_TimeElapsed++;
			EventManager.RaiseEvent(this, new TaskProgressAdvancedEventArgs(
				Misc.Round(_RequiredTime.Value - _TimeElapsed)));
		}

		public abstract void Execute();

		//protected Impact _DirectImpact;
		//public Impact DirectImpact
		//{
		//	get => _DirectImpact;
		//	set
		//	{
		//		if (value != _DirectImpact)
		//		{
		//			_DirectImpact = value;
		//			_DirectImpact.PropertyChanged += (sender, e) =>
		//				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectImpact)));
		//			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectImpact)));
		//		}
		//	}
		//}
		public enum Types
		{
			Census = 1,
			Policy = 2,
			Manufacture = 4,
			Transportation = 8,
			Deployment = 16,
			MT = 12,
			TD = 24,
			MTD = 28
		}
	}

	public class Census : Task
	{
		public Action<Task> DoCensus;

		public override void Execute() => DoCensus(this);

		public static void UpdateMyIndices(Census me, int valueIndex)
		{
			switch (valueIndex)
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
					me._LongTermEffect = new Effect(economy: new Expression(1, p => 1 + p * Data.Me.AdjustedBureaucracyIndex));
					me._RequiredTime = new Expression(2, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;

				default:
					me._LongTermEffect = new Effect();
					me._RequiredTime = new Expression(1, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;
			}

			Data.Me.OutdatedNationalIndices[valueIndex].Update(Data.Me.NationalIndices[valueIndex]);
		}
	}

	public class Policy : Task
	{
		private Action _Action;

		private Policy() { }
		public static Policy Create(Action action, Effect longTermEffect, Expression requiredTime)
		{
			return new Policy
			{
				_LongTermEffect = longTermEffect,
				_RequiredTime = requiredTime,
				_Action = action
			};
		}

		public override void Execute() => _Action();
	}

	public class Manufacture : Task
	{
		private Manufacture() { }
		public static Manufacture Create()
		{
			throw new NotImplementedException();
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}

	public class Transportation : Task
	{
		private Transportation() { }
		public static Transportation Create()
		{
			throw new NotImplementedException();
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}

	public class Deployment : Task
	{
		private Deployment() { }
		public static Deployment Create(Region destination, IDeployable deployable)
		{
			Deployment deployment = new Deployment();
			deployment.DeployableObject = deployable;
			deployment._Execute = () => deployment.DeployableObject.DeployedRegion = destination;
			throw new NotImplementedException();
		}

		private Action _Execute;
		public IDeployable DeployableObject { get; private set; } = null;

		public override void Execute()
		{
			_Execute();
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
