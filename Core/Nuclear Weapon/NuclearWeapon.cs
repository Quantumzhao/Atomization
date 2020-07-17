using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.ComponentModel;
using System;
using System.Xml;

namespace LCGuidebook.Core
{
	public abstract class NuclearWeapon : IDeployable
	{
		protected NuclearWeapon()
		{
			SelfDestroyed += () => Warheads.ForEach(wh => wh.DestroyThis());
		}

		public event Action SelfDestroyed;

		public ConstrainedList<Warhead> Warheads { get; } = new ConstrainedList<Warhead>();
		public Region DeployedRegion { get; set; }

		private bool _IsActivated = false;
		public bool IsActivated
		{
			get => _IsActivated;
			set
			{
				if (value && !_Platform.IsActivated)
				{
					throw new InvalidOperationException(Misc.BAD_ACTIVATION_SEQUENCE);
				}
				else
				{
					_IsActivated = value;
				}
			}
		}
		private string _Name;
		public string Name
		{
			get => _Name;
			set
			{
				if (value != _Name)
				{
					_Name = value;
					//OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
				}
			}
		}

		private Platform _Platform;
		public Platform Platform
		{
			get => _Platform;
			set
			{
				if (value != _Platform)
				{
					_Platform = value;
					//OnPropertyChanged(new PropertyChangedEventArgs(nameof(Platform)));
				}
			}
		}

		private Region _Target;
		public Region Target
		{
			get => _Target;
			set
			{
				if (value != _Target)
				{
					_Target = value;
					//OnPropertyChanged(new PropertyChangedEventArgs(nameof(Target)));
				}
			}
		}

		public string UID { get; } = GameManager.GenerateUID();

		public void DestroyThis() => SelfDestroyed?.Invoke();
	}

	public enum CarrierType
	{
		IRBM, 
		ICBM, 
		AerialBomb
	}

	//public abstract class NuclearMissile : NuclearWeapon
	//{
	//}

	//public class IRBM : NuclearMissile
	//{
	//}

	//public class ICBM : NuclearMissile
	//{
	//}

	//public class NuclearBomb : NuclearWeapon
	//{
	//}
}
