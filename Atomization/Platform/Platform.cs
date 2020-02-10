using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using Atomization.DataStructures;
using System;

namespace Atomization
{
	public abstract class Platform : IDestroyable, IUniqueObject
	{
		protected Platform()
		{
			UID = GameManager.GenerateUID();

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

			SelfDestroyed += () => NuclearWeapons.ForEach(w => w.DestroyThis());
		}

		public Region DeployedRegion { get; set; }
		public int AvailableLoad => NuclearWeapons.MaxCapacity - NuclearWeapons.Count;
		public ConstrainedList<NuclearWeapon> NuclearWeapons { get; set; } = new ConstrainedList<NuclearWeapon>();
		public string UID { get; }

		public event Action SelfDestroyed;

		public static void InitializeStaticMember()
		{
			Silo.InitializeStaticMember();
			StrategicBomber.InitializeStaticMember();
			MissileLauncher.InitializeStaticMember();
			NuclearSubmarine.InitializeStaticMember();
		}

		public void DestroyThis()
		{
			SelfDestroyed?.Invoke();
		}

		public enum Types
		{
			Silo,
			StrategicBomber,
			MissileLauncher,
			NuclearSubmarine
		}
	}

	public class Silo : Platform
	{

		public Silo() : base()
		{
			// don't delete this ↓
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -2, rawMaterial: -5);
			NuclearWeapons.MaxCapacity = 1;
		}

		public static CostOfStage Manufacture { get; private set; }
		public static CostOfStage Transportation { get; private set; }
		public static CostOfStage Deployment { get; private set; }
		public static CostOfStage Maintenance { get; private set; }

		public static new void InitializeStaticMember()
		{
			Manufacture = new CostOfStage(
				requiredTime: (Expression)4,
				longTermEffect: new Effect(economy: (Expression)(-2), rawMaterial: (Expression)(-5)),
				shortTermEffect: new Effect(economy: (Expression)(-40), rawMaterial: (Expression)(-60))
			);
		}
	}

	public class StrategicBomber : Platform
	{
		public static CostOfStage Manufacture { get; private set; }
		public static CostOfStage Transportation { get; private set; }
		public static CostOfStage Deployment { get; private set; }
		public static CostOfStage Maintenance { get; private set; }

		public StrategicBomber() : base()
		{
			NuclearWeapons.MaxCapacity = 1;
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -10, rawMaterial: -30);
		}

		public static new void InitializeStaticMember()
		{
			Manufacture = new CostOfStage(
				requiredTime: (Expression)7,
				shortTermEffect: new Effect(economy: (Expression)(-30), rawMaterial: (Expression)(-5)),
				longTermEffect: new Effect(economy: (Expression)(-10), rawMaterial: (Expression)(-30))
			);
		}
	}

	public class MissileLauncher : Platform
	{
		public static CostOfStage Manufacture { get; private set; }
		public static CostOfStage Transportation { get; private set; }
		public static CostOfStage Deployment { get; private set; }
		public static CostOfStage Maintenance { get; private set; }

		public MissileLauncher() : base()
		{
			NuclearWeapons.MaxCapacity = 1;
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -6, rawMaterial: -4);
		}

		public static new void InitializeStaticMember()
		{
			Manufacture = new CostOfStage(
				requiredTime: (Expression)6,
				shortTermEffect: new Effect(economy: (Expression)(-45), rawMaterial: (Expression)(-8)),
				longTermEffect: new Effect(economy: (Expression)(-6), rawMaterial: (Expression)(-4))
			);
		}
	}

	public class NuclearSubmarine : Platform
	{
		public static CostOfStage Manufacture { get; private set; }
		public static CostOfStage Transportation { get; private set; }
		public static CostOfStage Deployment { get; private set; }
		public static CostOfStage Maintenance { get; private set; }

		public NuclearSubmarine() : base()
		{
			NuclearWeapons.MaxCapacity = 8;
			//LongTermImpact = new Effect("Nuclear Arsenal Maintenance", economy: -15, rawMaterial: -10, nuclearMaterial: -0.05);
		}

		public static new void InitializeStaticMember()
		{
			Manufacture = new CostOfStage(
				requiredTime: (Expression)12,
				shortTermEffect: new Effect(economy: (Expression)(-100), rawMaterial: (Expression)(-200),
					nuclearMaterial: (Expression)(-1)),
				longTermEffect: new Effect(economy: (Expression)(-15), rawMaterial: (Expression)(-10),
					nuclearMaterial: (Expression)(-0.05))
			);
		}
	}
}
