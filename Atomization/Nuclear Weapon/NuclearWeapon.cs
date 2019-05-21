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
		public GameObjectList<Warhead> Warheads { get; set; } = new GameObjectList<Warhead>();
		public string WarheadType => Warheads.Count != 1 ? "(Multiple Types)" : Warheads[0].WarheadType;
	}

	public abstract class NuclearMissile : NuclearWeapon
	{
		public abstract string TypeName { get; }
	}

	public class CruiseMissile : NuclearMissile
	{
		public override string TypeName => "CruiseMissile";
	}

	public class MediumRangeMissile : NuclearMissile
	{
		public override string TypeName => "MediumRangeMissile";
	}

	public class ICBM : NuclearMissile
	{
		public override string TypeName => "ICBM";
	}

	public class NuclearBomb : NuclearWeapon
	{
		public string TypeName => "NuclearBomb";
	}

}
