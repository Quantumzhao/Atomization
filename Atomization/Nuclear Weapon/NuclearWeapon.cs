using Atomization.DataStructures;
using System.ComponentModel;

namespace Atomization
{
	public abstract class NuclearWeapon : ConstructableObject
	{
		protected NuclearWeapon()
		{
		}

		private string name;
		public string Name
		{
			get => name;
			set
			{
				if (value != name)
				{
					name = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
				}
			}
		}

		private Platform platform;
		public Platform Platform
		{
			get => platform;
			set
			{
				if (value != platform)
				{
					platform = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Platform)));
				}
			}
		}

		private Region target;
		public Region Target
		{
			get => target;
			set
			{
				if (value != target)
				{
					target = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Target)));
				}
			}
		}

		public ConstrainedList<Warhead> Warheads { get; set; } = new ConstrainedList<Warhead>();
		public string WarheadType => Warheads.Count != 1 ? "(Multiple Types)" : Warheads[0].WarheadType;
	}

	public abstract class NuclearMissile : NuclearWeapon
	{
	}

	public class CruiseMissile : NuclearMissile
	{
		public CruiseMissile() : base()
		{
			_BuildTime = 2;
			_LongTermImpact = new Impact("Nuclear Arsenal Maintenance", economy: -4, rawMaterial: -0.1);
			_DirectImpact = new Impact("Cruis Missile Construction", economy: -20, rawMaterial: -30);

			Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}

		public override string TypeName => "CruiseMissile";
	}

	public class MediumRangeMissile : NuclearMissile
	{
		public MediumRangeMissile() : base()
		{
			_BuildTime = 4;
			_LongTermImpact = new Impact("Nuclear Arsenal Maintenance", economy: -8, rawMaterial: -0.2);
			DirectImpact = new Impact("Medium Range Missile Construction", economy: -40, rawMaterial: -70);

			Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}
		public override string TypeName => "MediumRangeMissile";
	}

	public class ICBM : NuclearMissile
	{
		public ICBM() : base()
		{
			_BuildTime = 6;
			DirectImpact = new Impact("ICBM Construction", economy: -80, rawMaterial: -100);
			LongTermImpact = new Impact("Nuclear Arsenal Maintenance", economy: -20, rawMaterial: -1);

			Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}
		public override string TypeName => "ICBM";
	}

	public class NuclearBomb : NuclearWeapon
	{
		public NuclearBomb() : base()
		{
			_BuildTime = 1;
			DirectImpact = new Impact("Nuclear Bomb Construction", economy: -20, rawMaterial: -10);
			LongTermImpact = new Impact("Nuclear Arsenal Maintenance", economy: -1, rawMaterial: -0.05);

			Data.Me.ExpenditureAndRevenue.Add(LongTermImpact);
		}

		public override string TypeName => "NuclearBomb";
	}
}
