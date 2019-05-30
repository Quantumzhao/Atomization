﻿using System;
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
		public abstract Cost BuildCost { get; set; }
		public abstract Cost Maintenance { get; set; }
		public int AvailableLoad => NuclearWeapons.Capacity - NuclearWeapons.Count;
		public GameObjectList<NuclearWeapon> NuclearWeapons { get; set; } = new GameObjectList<NuclearWeapon>();
	}

	public class Silo : Platform
	{
		public Silo(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 1;

			BuildCost = new Cost(this, "Missile Silo Construction", economy: -40, rawMaterial: -60);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -2, rawMaterial: -5);
		}

		public override string TypeName => "Silo";
		public override int BuildTime { get; set; } = 4;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}

	public class StrategicBomber : Platform
	{
		public StrategicBomber(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 1;

			BuildCost = new Cost(this, "Strategic Bomber Construction", economy: -30, rawMaterial: -5);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -10, rawMaterial: -30);
		}
		public override string TypeName => "StrategicBomber";

		public override int BuildTime { get; set; } = 7;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}

	public class MissileLauncher : Platform
	{
		public MissileLauncher(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 1;

			BuildCost = new Cost(this, "Missile Launcher Construction", economy: -45, rawMaterial: -8);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -6, rawMaterial: -4);
		}
		public override string TypeName => "MissileLauncher";

		public override int BuildTime { get; set; } = 6;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}

	public class NuclearSubmarine : Platform
	{
		public NuclearSubmarine(NotifyCollectionChangedEventHandler onCollectionChanged = null)
			: base(onCollectionChanged)
		{
			NuclearWeapons.Capacity = 8;

			BuildCost = new Cost(this, "Nuclear Submarine Construction", economy: -100, rawMaterial: -200, nuclearMaterial: -1);
			Maintenance = new Cost(this, "Nuclear Arsenal Maintenance", economy: -15, rawMaterial: -10, nuclearMaterial: -0.05);
		}

		public override string TypeName => "NuclearSubmarine";

		public override int BuildTime { get; set; } = 12;
		public override Cost BuildCost { get; set; }
		public override Cost Maintenance { get; set; }
	}
}
