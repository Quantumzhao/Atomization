using Atomization.DataStructures;
using System.ComponentModel;
using System;

namespace Atomization
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

	public class CruiseMissile : NuclearMissile
	{
		public CruiseMissile() : base()
		{
			//_BuildTime = 2;
			//_LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -4, rawMaterial: -0.1);
			//_DirectImpact = new Effect("Cruis Missile Construction", economy: -20, rawMaterial: -30);

			//Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}

		//public override string TypeName => "CruiseMissile";
	}

	public class MediumRangeMissile : NuclearMissile
	{
		public MediumRangeMissile() : base()
		{
			//_BuildTime = 4;
			//_LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -8, rawMaterial: -0.2);
			//DirectImpact = new Effect("Medium Range Missile Construction", economy: -40, rawMaterial: -70);

			//Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}
		//public override string TypeName => "MediumRangeMissile";
	}

	public class ICBM : NuclearMissile
	{
		public ICBM() : base()
		{
			//_BuildTime = 6;
			//DirectImpact = new Effect("ICBM Construction", economy: -80, rawMaterial: -100);
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -20, rawMaterial: -1);

			//Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}
		//public override string TypeName => "ICBM";
	}

	public class NuclearBomb : NuclearWeapon
	{
		public NuclearBomb() : base()
		{
			//_BuildTime = 1;
			//DirectImpact = new Effect("Nuclear Bomb Construction", economy: -20, rawMaterial: -10);
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -1, rawMaterial: -0.05);

			//Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}

		//public override string TypeName => "NuclearBomb";
	}
}
