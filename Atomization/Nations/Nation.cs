using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

			Economy = new ValueComplex(this, 20000);		// x10^9
			HiEduPopu = new ValueComplex(this, 20000);		// x10^6
			Army = new ValueComplex(this, 50000);			// x10^3
			Navy = new ValueComplex(this, 5000);			// x10^3
			Food = new ValueComplex(this, 20000);			// x10^6
			RawMaterial = new ValueComplex(this, 4000);		// x10^3
			NuclearMaterial = new ValueComplex(this, 100);	// x10^3
			Stability = new ValueComplex(this, 75) { Maximum = new InternalValue(this, 100) };
		}

		public const int NumOfNonAdjacentNations = 5;

		#region Value Definitions
		public Waters TerritorialWaters { get; }
		public ValueComplex Economy { get; set; }
		public ValueComplex HiEduPopu { get; set; }
		public ValueComplex Army { get; set; }
		public ValueComplex Navy { get; set; }
		public ValueComplex Food { get; set; }
		public ValueComplex RawMaterial { get; set; }
		public ValueComplex NuclearMaterial { get; set; }
		public ValueComplex Stability { get; set; }
		#endregion
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
		}

		// null stands for independence
		public Superpower Affiliation { get; set; } = null;
	}
	public class Superpower : Nation
	{
		public const int InitialNukeSilos = 10;
		public Superpower(NotifyCollectionChangedEventHandler onCollectionChanged = null) : base()
		{
			for (int i = 0; i < InitialNukeSilos; i++)
			{
				NuclearPlatforms.Add(new Silo(onCollectionChanged) { DeployRegion = this});
			}

			for (int i = 0; i < NumOfAdjacentNations; i++)
			{
				RegularNation nation = new RegularNation() { Name = Data.NationNames.Dequeue() };
				Data.Regions.Add(nation);
				Adjacency[i] = nation;
			}

			Economy.Growth.Add("Army Maintenance", (float)(-0.001 * Army.Value));
			Economy.Growth.Add("Navy Maintenance Cost", (float)(-0.005 * Navy.Value));
			Economy.Growth.Add("Domestic Development", -0.9);
			Economy.Growth.Add("Government Revenue", 20000);

			HiEduPopu.Growth.Add("Graduates", 1000);

			Army.Growth.Add("Army Expansion", 0.01);

			Navy.Growth.Add("Navy Expansion", 0.005);

			Food.Growth.Add("Domestic Production", 42000);
			Food.Growth.Add("Domestic Consumption", -2.0);

			RawMaterial.Growth.Add("Domestic Production", 10200);
			RawMaterial.Growth.Add("Industrial Consumption", -10000);

			NuclearMaterial.Growth.Add("Production", 1);

			string title = "Nuclear Arsenal Maintenance";
			foreach (var p in NuclearPlatforms)
			{
				var c = p.BuildCost;
				if (c.Economy != null) Economy.Growth.Add(title, (float)c.Economy.Value);
				if (c.HiEduPopu != null) HiEduPopu.Growth.Add(title, (float)c.HiEduPopu.Value);
				if (c.Army != null) Army.Growth.Add(title, (float)c.Army.Value);
				if (c.Navy != null) Navy.Growth.Add(title, (float)c.Navy.Value);
				if (c.Food != null) Food.Growth.Add(title, (float)c.Food.Value);
				if (c.RawMaterial != null) RawMaterial.Growth.Add(title, (float)c.RawMaterial.Value);
				if (c.NuclearMaterial != null) NuclearMaterial.Growth.Add(title, (float)c.NuclearMaterial.Value);
				if (c.Stability != null) Stability.Growth.Add(title, (float)c.Stability.Value);

			}
		}
		public const int NumOfAdjacentNations = 5;

		public GameObjectList<Platform> NuclearPlatforms { get; set; } = new GameObjectList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NumOfAdjacentNations];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();
	}
}
