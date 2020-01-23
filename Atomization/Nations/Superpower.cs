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
			Superpower superpower = new Superpower
			{
				Name = name
			};

			superpower.NationalIndices.Economy.CurrentValue = 20000 + 400;  // x10^9
			superpower.NationalIndices.Population.CurrentValue = 20000;      // x10^6
			superpower.NationalIndices.Army.CurrentValue = 50000;           // x10^3
			superpower.NationalIndices.Navy.CurrentValue = 5000;            // x10^3
			superpower.NationalIndices.Food.CurrentValue = 20000;           // x10^6
			superpower.NationalIndices.RawMaterial.CurrentValue = 4000;     // x10^3
			superpower.NationalIndices.NuclearMaterial.CurrentValue = 100;  // x10^3
			superpower.NationalIndices.Stability.CurrentValue = 75;
			superpower.NationalIndices.Bureaucracy.CurrentValue = 10;

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

		public static class Nuclear
		{
			public static void DeployNewNuclearStrikePlatform(Platform.Types type, Region region)
			{
				//Data.Me.TaskSequence.AddNewTask(Task.Types.MTD, $"Deploying a new {type}");
				throw new NotImplementedException();
			}


			public static void DestroyNuke(NuclearWeapon nuclearWeapon)
			{
				Data.Me.TaskSequence.AddNewTask(Task.Types.TD, $"Destroying {nuclearWeapon}", null);

				EventManager.TaskProgressAdvenced += RemoveNuke;
				throw new NotImplementedException();
			}
			private static void RemoveNuke(Task sender, TaskProgressAdvancedEventArgs e)
			{
				if (sender is Deployment deployment && )
				{

				}
				throw new NotImplementedException();
			}
		}

		public static class Operations
		{
			public static void InvadeAndStation(Nation target, double forcesFromMe, double forcesFromAlliance, 
				bool doSupportaPuppet = false)
			{

			}

			public static void Assinate(Nation target, PublicFigures figure)
			{
				throw new NotImplementedException();
			}

			public enum PublicFigures
			{
				
			}
		}

		public static class Intelligence
		{

		}

		public static class Defence
		{

		}

		public static class Technology
		{

		}

		public static class Domestic
		{

		}

		public static class Diplomacy
		{

		}
	}
}