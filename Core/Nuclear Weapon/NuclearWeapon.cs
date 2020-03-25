using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.ComponentModel;
using System;

namespace LCGuidebook.Core
{
	public abstract class NuclearWeapon : IDeployable, IDestroyable
	{
		protected NuclearWeapon()
		{
			SelfDestroyed += () => Warheads.ForEach(wh => wh.DestroyThis());
		}

		public event Action SelfDestroyed;

		public ConstrainedList<Warhead> Warheads { get; } = new ConstrainedList<Warhead>();
		public string WarheadType => Warheads.Count != 1 ? "(Multiple Types)" : Warheads[0].WarheadType;
		public Region DeployedRegion { get; set; }
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
		private bool _IsActivated = false;

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

		public void DestroyThis() => SelfDestroyed?.Invoke();
	}

	public abstract class NuclearMissile : NuclearWeapon
	{
	}

	public class IRBM : NuclearMissile
	{
	}

	public class ICBM : NuclearMissile
	{
	}

	public class NuclearBomb : NuclearWeapon
	{
	}
}
