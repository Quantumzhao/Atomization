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

		public GameObjectList<Warhead> Warheads { get; set; } = new GameObjectList<Warhead>();
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
			maintenance = new Cost("Nuclear Arsenal Maintenance", economy: -4, rawMaterial: -0.1);
			buildCost = new Cost("Cruis Missile Construction", economy: -20, rawMaterial: -30);

			if (IsMine)
			{
				Data.Me.ExpenditureAndRevenue.Add(Maintenance);
			}
		}

		public override string TypeName => "CruiseMissile";
	}

	public class MediumRangeMissile : NuclearMissile
	{
		public MediumRangeMissile() : base()
		{
			_BuildTime = 4;
			maintenance = new Cost("Nuclear Arsenal Maintenance", economy: -8, rawMaterial: -0.2);
			BuildCost = new Cost("Medium Range Missile Construction", economy: -40, rawMaterial: -70);

			if (IsMine)
			{
				Data.Me.ExpenditureAndRevenue.Add(Maintenance);
			}
		}
		public override string TypeName => "MediumRangeMissile";
	}

	public class ICBM : NuclearMissile
	{
		public ICBM() : base()
		{
			_BuildTime = 6;
			BuildCost = new Cost("ICBM Construction", economy: -80, rawMaterial: -100);
			Maintenance = new Cost("Nuclear Arsenal Maintenance", economy: -20, rawMaterial: -1);

			if (IsMine)
			{
				Data.Me.ExpenditureAndRevenue.Add(Maintenance);
			}
		}
		public override string TypeName => "ICBM";
	}

	public class NuclearBomb : NuclearWeapon
	{
		public NuclearBomb() : base()
		{
			_BuildTime = 1;
			BuildCost = new Cost("Nuclear Bomb Construction", economy: -20, rawMaterial: -10);
			Maintenance = new Cost("Nuclear Arsenal Maintenance", economy: -1, rawMaterial: -0.05);

			if (IsMine)
			{
				Data.Me.ExpenditureAndRevenue.Add(Maintenance);
			}
		}

		public override string TypeName => "NuclearBomb";
	}
}
