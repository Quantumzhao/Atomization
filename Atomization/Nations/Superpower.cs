using Atomization.DataStructures;
using System.Collections.Generic;
using System;

namespace Atomization
{
	public class Superpower : Nation
	{
		public const int INITIAL_NUKE_SILOS = 10;
		public const int NUM_ADJACENT_NATIONS = 5;

		public Superpower() : base()
		{
			for (int i = 0; i < NUM_ADJACENT_NATIONS; i++)
			{
				RegularNation nation = new RegularNation() { Name = Data.NationNames.Dequeue() };
				Data.Regions.Add(nation);
				// initialize the no. of adj nations
				Adjacency[i] = nation;
			}

			Affiliation = this;
		}

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public static Superpower InitializeMe(string name)
		{
			Superpower superpower = new Superpower();

			superpower.Name = name;
			superpower.Economy = new ValueComplex(20000 + 400);  // x10^9
			superpower.Population = new ValueComplex(20000);      // x10^6
			superpower.Army = new ValueComplex(50000);           // x10^3
			superpower.Navy = new ValueComplex(5000);            // x10^3
			superpower.Food = new ValueComplex(20000);           // x10^6
			superpower.RawMaterial = new ValueComplex(4000);     // x10^3
			superpower.NuclearMaterial = new ValueComplex(100);  // x10^3
			superpower.Stability = new ValueComplex(75) { Maximum = 100 };
			superpower.Bureaucracy = new ValueComplex(10);

			for (int i = 0; i < INITIAL_NUKE_SILOS; i++)
			{
				superpower.NuclearPlatforms.Add(new Silo(superpower));
			}

			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect(
			//		"Army Maintenance",
			//		new Expression(superpower.Army.CurrentValue, v => -0.001 * v)
			//		)
			//	);
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect(
			//		"Navy Maintenance",
			//		economy: new Expression(superpower.Navy.CurrentValue, v => -0.005 * v)
			//		)
			//	);
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect(
			//		"Domestic Development",
			//		new Expression(superpower.Economy.CurrentValue, v => -0.9 * v)
			//	)
			//);
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect("Government Revenue", economy: 20000));
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect("Graduates", hiEduPopu: 1000));
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect("Domestic Production", food: 42000));
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect(
			//		"Domestic Consumption",
			//		food: new Expression(superpower.HiEduPopu.CurrentValue, v => -2 * v)
			//	)
			//);
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect("Domestic Production", rawMaterial: 10200));
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect("Industrial Consumption", rawMaterial: -10000));
			//superpower.ExpenditureAndRevenue.Add(
			//	new Effect("Production", nuclearMaterial: 1));

			return superpower;
		}
	}
}