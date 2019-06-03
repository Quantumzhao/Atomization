using System.Collections.Generic;
using System.Collections.Specialized;

namespace Atomization
{
	public abstract class Region
	{
		public string Name { get; set; }
	}

	public class Waters : Region
	{
		public const int NumOfInternationalWaters = 5;
		public Nation Affiliation { get; set; }
	}

	public abstract class Nation : Region
	{
		public Nation()
		{
			TerritorialWaters = new Waters() { Name = Data.WatersNames.Dequeue() };
			TerritorialWaters.Affiliation = this;
			Data.Regions.Add(TerritorialWaters);
			ExpenditureAndRevenue.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					AddExpenditureAndRevenue(e.NewItems[0] as Cost);
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					RemoveExpenditureAndRevenue(e.OldItems[0] as Cost);
				}
			};
		}

		// null stands for independence
		public Superpower Affiliation { get; set; } = null;

		public const int NumOfNonAdjacentNations = 5;
		public Waters TerritorialWaters { get; }

		#region Value Definitions
		protected ValueComplex[] Values = new ValueComplex[8];
		protected GameObjectList<Cost> ExpenditureAndRevenue = new GameObjectList<Cost>();
		public ValueComplex Economy
		{
			get => Values[0];
			protected set => Values[0] = value;
		}
		public ValueComplex HiEduPopu
		{
			get => Values[1];
			protected set => Values[1] = value;
		}
		public ValueComplex Army
		{
			get => Values[2];
			protected set => Values[2] = value;
		}
		public ValueComplex Navy
		{
			get => Values[3];
			protected set => Values[3] = value;
		}
		public ValueComplex Food
		{
			get => Values[4];
			protected set => Values[4] = value;
		}

		public ValueComplex RawMaterial
		{
			get => Values[5];
			protected set => Values[5] = value;
		}

		public ValueComplex NuclearMaterial
		{
			get => Values[6];
			protected set => Values[6] = value;
		}

		public ValueComplex Stability
		{
			get => Values[7];
			protected set => Values[7] = value;
		}
		#endregion

		private void AddExpenditureAndRevenue(Cost cost)
		{
			for (int i = 0; i < cost.Values.Count; i++)
			{
				if (cost.Values[i].Key.Value != 0)
				{
					if (cost.Values[i].Value)
					{
						Values[i].Growth.Add_AbsValue(cost.Name, cost.Values[i].Key.Value);
					}
					else
					{
						Values[i].Growth.Add_Percent(cost.Name, cost.Values[i].Key.Value);
					}
					cost.Values.CollectionChanged += this.Values[i].Growth.OnCollectionChanged;
				}
			}
		}
		private void RemoveExpenditureAndRevenue(Cost cost)
		{
			for (int i = 0; i < cost.Values.Count; i++)
			{
				if (cost.Values[i].Key.Value != 0)
				{

				}
			}
		}
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
		}
	}
	public class Superpower : Nation
	{
		public const int InitialNukeSilos = 10;
		public Superpower() : base()
		{
			for (int i = 0; i < NumOfAdjacentNations; i++)
			{
				RegularNation nation = new RegularNation() { Name = Data.NationNames.Dequeue() };
				Data.Regions.Add(nation);
				Adjacency[i] = nation;
			}

			Affiliation = this;
		}
		public const int NumOfAdjacentNations = 5;

		public GameObjectList<Platform> NuclearPlatforms { get; set; } = new GameObjectList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NumOfAdjacentNations];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public static Superpower InitializeMe(string name)
		{
			Superpower superpower = new Superpower();

			superpower.Name = name;
			superpower.Economy = new ValueComplex(20000);        // x10^9
			superpower.HiEduPopu = new ValueComplex(20000);      // x10^6
			superpower.Army = new ValueComplex(50000);           // x10^3
			superpower.Navy = new ValueComplex(5000);            // x10^3
			superpower.Food = new ValueComplex(20000);           // x10^6
			superpower.RawMaterial = new ValueComplex(4000);     // x10^3
			superpower.NuclearMaterial = new ValueComplex(100);  // x10^3
			superpower.Stability = new ValueComplex(75) { Maximum = new InternalValue(100) };

			for (int i = 0; i < InitialNukeSilos; i++)
			{
				superpower.NuclearPlatforms.Add(new Silo(superpower));
			}

			superpower.AddExpenditureAndRevenue(
				new Cost("Army Maintenance", economy: -0.001 * superpower.Army.Value));
			superpower.AddExpenditureAndRevenue(
				new Cost("Navy Maintenance", economy: -0.005 * superpower.Navy.Value));
			superpower.AddExpenditureAndRevenue(
				new Cost("Domestic Development", economy: -0.9, isEconomyAbs: false));
			superpower.AddExpenditureAndRevenue(
				new Cost("Government Revenue", economy: 20000));
			superpower.AddExpenditureAndRevenue(
				new Cost("Graduates", hiEduPopu: 1000));
			superpower.AddExpenditureAndRevenue(
				new Cost("Domestic Production", food: 42000));
			superpower.AddExpenditureAndRevenue(
				new Cost("Domestic Consumption", food: -2.0 * superpower.HiEduPopu.Value));
			superpower.AddExpenditureAndRevenue(
				new Cost("Domestic Production", rawMaterial: 10200));
			superpower.AddExpenditureAndRevenue(
				new Cost("Industrial Consumption", rawMaterial: -10000));
			superpower.AddExpenditureAndRevenue(
				new Cost("Production", nuclearMaterial: 1));
			
			return superpower;
		}
	}
}
