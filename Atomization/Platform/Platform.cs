using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public abstract class Platform
	{
		public Platform(
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemAdded = null,
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemRemoved = null
		)
		{
			NuclearWeapons.OnItemAdded += onItemAdded;
			NuclearWeapons.OnItemRemoved += onItemRemoved;
		}

		public abstract string TypeName { get; }
		public Region DeployRegion { get; set; }
		public int AvailableLoad => NuclearWeapons.Capacity - NuclearWeapons.Count;
		public GameObjectList<NuclearWeapon> NuclearWeapons { get; set; } = new GameObjectList<NuclearWeapon>();
	}

	public class Silo : Platform
	{
		public Silo(
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemAdded = null,
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemRemoved = null
		) : base(onItemAdded: onItemAdded, onItemRemoved: onItemRemoved)
		{
			NuclearWeapons.Capacity = 1;
		}

		public override string TypeName => "Silo";
	}

	public class StrategicBomber : Platform
	{
		public StrategicBomber(
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemAdded = null,
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemRemoved = null
		) : base(onItemAdded: onItemAdded, onItemRemoved: onItemRemoved)
		{
			NuclearWeapons.Capacity = 1;
		}
		public override string TypeName => "StrategicBomber";
	}

	public class MissileLauncher : Platform
	{
		public MissileLauncher(
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemAdded = null,
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemRemoved = null
		) : base(onItemAdded: onItemAdded, onItemRemoved: onItemRemoved)
		{
			NuclearWeapons.Capacity = 1;
		}
		public override string TypeName => "MissileLauncher";
	}

	public class NuclearSubmarine : Platform
	{
		public NuclearSubmarine(
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemAdded = null,
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemRemoved = null
		) : base(onItemAdded: onItemAdded, onItemRemoved: onItemRemoved)
		{
			NuclearWeapons.Capacity = 8;
		}

		public override string TypeName => "NuclearSubmarine";
	}
}
