using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public abstract class NuclearWeapon
	{
		public string Name { get; set; }
		public Platform Platform { get; set; }
		public Region Target { get; set; }
		public abstract int BuildTime { get; set; }
		public abstract Cost BuildCost { get; set; }
		public abstract Cost Maintenance { get; set; }
		public abstract string TypeName { get; }
		public GameObjectList<Warhead> Warheads { get; set; } = new GameObjectList<Warhead>();
		public string WarheadType => Warheads.Count != 1 ? "(Multiple Types)" : Warheads[0].WarheadType;
	}

	public abstract class NuclearMissile : NuclearWeapon
	{
	}

	public class CruiseMissile : NuclearMissile
	{
		public CruiseMissile()
		{
			BuildCost = new Cost(this, "Cruis Missile Construction", economy: -20, rawMaterial: -30);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -4, rawMaterial: -0.1);
		}

		public override string TypeName => "CruiseMissile";
		public override int BuildTime { get; set; } = 2;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}

	public class MediumRangeMissile : NuclearMissile
	{
		public MediumRangeMissile()
		{
			BuildCost = new Cost(this, "Medium Range Missile Construction", economy: -40, rawMaterial: -70);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -8, rawMaterial: -0.2);
		}
		public override string TypeName => "MediumRangeMissile";
		public override int BuildTime { get; set; } = 4;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}

	public class ICBM : NuclearMissile
	{
		public ICBM()
		{
			BuildCost = new Cost(this, "ICBM Construction", economy: -80, rawMaterial: -100);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -20, rawMaterial: -1);
		}
		public override string TypeName => "ICBM";
		public override int BuildTime { get; set; } = 6;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}

	public class NuclearBomb : NuclearWeapon
	{
		public NuclearBomb()
		{
			BuildCost = new Cost(this, "Nuclear Bomb Construction", economy: -20, rawMaterial: -10);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -1, rawMaterial: -0.05);
		}

		public override string TypeName => "NuclearBomb";
		public override int BuildTime { get; set; } = 1;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}
}
