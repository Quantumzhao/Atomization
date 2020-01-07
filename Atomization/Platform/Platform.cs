﻿using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using Atomization.DataStructures;

namespace Atomization
{
	public abstract class Platform : ConstructableObject
	{
		protected Platform() : base()
		{
			NuclearWeapons.CollectionChanged +=
			(sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					Data.MyNuclearWeapons.Add(e.NewItems[0] as NuclearWeapon);
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					Data.MyNuclearWeapons.Remove(e.OldItems[0] as NuclearWeapon);
				}
			};

			ConstructionCompleted += item =>
			{
				if (DeployRegion is Nation)
				{
					(DeployRegion as Nation).Affiliation.ExpenditureAndRevenue.Add(item.LongTermImpact);
				}
				else
				{
					(DeployRegion as Waters).Affiliation.Affiliation.ExpenditureAndRevenue.Add(item.LongTermImpact);
				}
			};
		}
		public Region DeployRegion { get; set; }
		public int AvailableLoad => NuclearWeapons.MaxCapacity - NuclearWeapons.Count;
		public ConstrainedList<NuclearWeapon> NuclearWeapons { get; set; } = new ConstrainedList<NuclearWeapon>();
	}

	public class Silo : Platform
	{
		public Silo(Nation deployRegion) : base()
		{
			_BuildTime = 4;

			DeployRegion = deployRegion;

			NuclearWeapons.MaxCapacity = 1;

			//DirectImpact = new Effect("Missile Silo Construction", economy: -40, rawMaterial: -60);
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -2, rawMaterial: -5);

			//(DeployRegion as Nation).Affiliation.AddCostOfExecution(DirectImpact);
		}

		public override string TypeName => "Silo";
	}

	public class StrategicBomber : Platform
	{
		public StrategicBomber(Nation deployRegion) : base()
		{
			_BuildTime = 7;

			DeployRegion = deployRegion;

			NuclearWeapons.MaxCapacity = 1;

			//DirectImpact = new Effect("Strategic Bomber Construction", economy: -30, rawMaterial: -5);
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -10, rawMaterial: -30);

			//(DeployRegion as Nation).Affiliation.AddCostOfExecution(DirectImpact);
		}
		public override string TypeName => "StrategicBomber";
	}

	public class MissileLauncher : Platform
	{
		public MissileLauncher(Nation deployRegion) : base()
		{
			_BuildTime = 6;

			DeployRegion = deployRegion;

			NuclearWeapons.MaxCapacity = 1;

			//DirectImpact = new Effect("Missile Launcher Construction", economy: -45, rawMaterial: -8);
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -6, rawMaterial: -4);

			//(DeployRegion as Nation).Affiliation.AddCostOfExecution(DirectImpact);
		}
		public override string TypeName => "MissileLauncher";
	}

	public class NuclearSubmarine : Platform
	{
		public NuclearSubmarine(Region deployRegion) : base()
		{
			_BuildTime = 12;

			DeployRegion = deployRegion;

			NuclearWeapons.MaxCapacity = 8;

			//DirectImpact = new Effect("Nuclear Submarine Construction", economy: -100, rawMaterial: -200, nuclearMaterial: -1);
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -15, rawMaterial: -10, nuclearMaterial: -0.05);

			//(DeployRegion as Nation).Affiliation.AddCostOfExecution(DirectImpact);
		}

		public override string TypeName => "NuclearSubmarine";
	}
}
