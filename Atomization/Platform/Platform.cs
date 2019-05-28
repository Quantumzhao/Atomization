using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Atomization
{
	public abstract class Platform
	{
		public Platform(NotifyCollectionChangedEventHandler onCollectionChanged = null)
		{
			//NuclearWeapons.OnItemAdded += onItemAdded;
			//NuclearWeapons.OnItemRemoved += onItemRemoved;
			NuclearWeapons.CollectionChanged += onCollectionChanged;
		}

		public abstract string TypeName { get; }
		public Region DeployRegion { get; set; }
		public abstract int BuildTime { get; set; }
		public abstract double BuildCost { get; set; }
		public abstract double Maintenance { get; set; }
		public int AvailableLoad => NuclearWeapons.Capacity - NuclearWeapons.Count;
		public GameObjectList<NuclearWeapon> NuclearWeapons { get; set; } = new GameObjectList<NuclearWeapon>();
	}

	public class Silo : Platform
	{
		public Silo(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 1;
		}

		public override string TypeName => "Silo";
		public override int BuildTime { get; set; } = 4;
		public override double BuildCost { get; set; } = 40;
		public override double Maintenance { get; set; }
	}

	public class StrategicBomber : Platform
	{
		public StrategicBomber(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 1;
		}
		public override string TypeName => "StrategicBomber";

		public override int BuildTime { get; set; } = 7;
		public override double BuildCost { get; set; }
		public override double Maintenance { get; set; }
	}

	public class MissileLauncher : Platform
	{
		public MissileLauncher(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 1;
		}
		public override string TypeName => "MissileLauncher";

		public override int BuildTime { get; set; } = 6;
		public override double BuildCost { get; set; }
		public override double Maintenance { get; set; }
	}

	public class NuclearSubmarine : Platform
	{
		public NuclearSubmarine(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 8;
		}

		public override string TypeName => "NuclearSubmarine";

		public override int BuildTime { get; set; } = 12;
		public override double BuildCost { get; set; }
		public override double Maintenance { get; set; }
	}
}
