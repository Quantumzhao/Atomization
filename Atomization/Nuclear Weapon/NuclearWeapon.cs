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
		public List<Warhead> Warheads { get; set; }
		public List<Maneuverability> Maneuverabilities { get; set; }
		public string Type => Warheads.Count != 1 ? "(Multiple Types)" : Warheads[0].GetType().Name;
	}

	public class NuclearMissile : NuclearWeapon
	{
		public string TypeName => "NuclearMissile";
	}

	public class NuclearBomb : NuclearWeapon
	{
	}

}
