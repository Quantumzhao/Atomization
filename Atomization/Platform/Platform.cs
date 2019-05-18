using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public abstract class Platform
	{
		public Nation DeployNation { get; set; }
		public int MaxCapacity { get; set; }
		public int AvailableLoad
		{
			get => MaxCapacity - NuclearWeapons.Count;
		}
		public List<NuclearWeapon> NuclearWeapons { get; set; } = new List<NuclearWeapon>();
	}

	public class Silo : Platform
	{
		public Silo()
		{
			MaxCapacity = 1;

			NuclearWeapons.Add(new NuclearMissile()
			{
				Platform = this,
				Warheads = new List<Warhead> { new Atomic() },
				Name = "Boom"
			});
		}

		public string TypeName => "Silo";
	}

	public class StrategicBomber : Platform
	{
	}
}
