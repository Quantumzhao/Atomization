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
		public int AvailableLoad => NuclearWeapons.Capacity - NuclearWeapons.Count;
		public GameObjectList<NuclearWeapon> NuclearWeapons { get; set; } = new GameObjectList<NuclearWeapon>();
	}

	public class Silo : Platform
	{
		public Silo()
		{
			NuclearWeapons.Capacity = 1;
		}

		public string TypeName => "Silo";
	}

	public class StrategicBomber : Platform
	{
	}
}
