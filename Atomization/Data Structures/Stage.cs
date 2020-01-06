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
	public class Stage
	{
		private Stage() { }
		public static Stage Create(StageType type)
		{
			throw new NotImplementedException();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ConfidentialLevel ConfidentialLevel { get; set; }
		public StageType StageType { get; set; }
		public Task HostTask { get; private set; }

		protected int _RemainingTime;
		public int RemainingTime
		{
			get => _RemainingTime;
			set
			{
				if (value == 0)
				{
					//ConstructionCompleted?.Invoke(this);
					_RemainingTime = -1;
					return;
				}
				else if (_RemainingTime == -1)
				{
					return;
				}

				if (value != _RemainingTime)
				{
					_RemainingTime = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RemainingTime)));
					if (value == 0)
					{
						HostTask.RemoveCurrentStage();
					}
				}
			}
		}

		protected Effect _LongTermEffect;
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

	public enum StageType
	{
		Census,
		Policy,
		Purchase,
		Manufacture,
		Transportation,
		Deployment
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
