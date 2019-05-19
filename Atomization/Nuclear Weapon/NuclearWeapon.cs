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
		public GameObjectList<Warhead> Warheads { get; set; } = new GameObjectList<Warhead>();
		public string WarheadType => Warheads.Count != 1 ? "(Multiple Types)" : Warheads[0].WarheadType;
	}

	public class NuclearMissile : NuclearWeapon
	{
		public string TypeName => "NuclearMissile";
	}

	public class NuclearBomb : NuclearWeapon
	{
		public string TypeName => "NuclearBomb";
	}

}
