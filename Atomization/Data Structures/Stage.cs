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
	public abstract class Stage : IExecutable
	{
		protected Stage() 
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
			EventManager.RaiseEvent(this, new StageProgressAdvancedEventArgs(
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
	}

	public class Census : Stage
	{
		private Census() { }
		public static Census Create(int nationalIndex)
		{
			var census = new Census();
			census.Init(nationalIndex);
			return census;
		}

		private Action _Execute;

		private void Init(int index)
		{
			// Wait for the implementation of tech tree system
			switch (index)
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
					_LongTermEffect = new Effect(economy: new Expression(1, p => 1 + p * Data.Me.AdjustedBureaucracyIndex));
					_RequiredTime = new Expression(2, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;

				default:
					_LongTermEffect = new Effect();
					_RequiredTime = new Expression(1, p => p * Data.Me.AdjustedBureaucracyIndex);
					break;
			}

			_Execute = () => UpdateMyIndices(index);
		}

		public override void Execute() => _Execute();

		private void UpdateMyIndices(int valueIndex) => Data.Me.OutdatedNationalIndices[valueIndex].Update(Data.Me.NationalIndices[valueIndex]);
	}

	public class Policy : Stage
	{
		private Policy() { }
		public static Policy Create()
		{
			throw new NotImplementedException();
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}

	public class Purchase : Stage
	{
		private Purchase() { }
		public static Purchase Create()
		{
			throw new NotImplementedException();
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}

	public class Manufacture : Stage
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

	public class Transportation : Stage
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

	public class Deployment : Stage
	{
		private Deployment() { }
		public static Deployment Create()
		{
			throw new NotImplementedException();
		}

		public override void Execute()
		{
			throw new NotImplementedException();
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
