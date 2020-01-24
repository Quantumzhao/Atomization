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

			Action<string, Effect> imposeEffect = (effectName, effect) =>
			{
				for (int i = 0; i < ValueComplexNTuple.NUM_VALUES; i++)
				{
					Data.Me.NationalIndices[i].Growth.AddTerm(effectName, effect[i]);
				}
			};

			for (int i = 0; i < INITIAL_NUKE_SILOS; i++)
			{
				superpower.NuclearPlatforms.Add(new Silo());
			}

			imposeEffect("Army Maintenance",
				new Effect(new Expression(superpower.NationalIndices.Army.CurrentValue, v => -0.001 * v)));
			imposeEffect("Navy Maintenance",
				new Effect(economy: new Expression(superpower.NationalIndices.Navy.CurrentValue, v => -0.005 * v)));
			imposeEffect("Domestic Development", 
				new Effect(new Expression(superpower.NationalIndices.Economy.CurrentValue, v => -0.9 * v)));
			imposeEffect("Government Revenue", new Effect(economy: (Expression)20000));
			imposeEffect("Graduates", new Effect(hiEduPopu: (Expression)1000));
			imposeEffect("Domestic Production", new Effect(food: (Expression)42000));
			imposeEffect("Domestic Consumption", 
				new Effect(food: new Expression(superpower.NationalIndices.Population.CurrentValue, v => -2 * v)));
			imposeEffect("Domestic Production", new Effect(rawMaterial: (Expression)10200));
			imposeEffect("Industrial Consumption", new Effect(rawMaterial: (Expression)(-10000)));
			imposeEffect("Production", new Effect(nuclearMaterial: (Expression)1));

			return superpower;
		}

		public static class Nuclear
		{
			//public static void DeployNewNuclearStrikePlatform(Platform.Types type, Region region)
			//{
			//	Data.Me.TaskSequence.AddNewTask(Task.Types.MTD, $"Deploying a new {type}");
			//	throw new NotImplementedException();
			//}

			public static void EnrollNukeStrikePlatfrom(Platform.Types type)
			{
				Manufacture manufacture;
				string name = $"Sending a new {type} to reserve";
				Effect longTermEffect;
				Effect shortTermEffect;
				Expression requiredTime;
				Func<Platform> instruction;

				switch (type)
				{
					case Platform.Types.Silo:
						longTermEffect = Silo.LongTermEffect_M;
						shortTermEffect = Silo.ShortTermEffect_M;
						requiredTime = Silo.RequiredTime_M;
						instruction = () => new Silo();
						break;

					case Platform.Types.StrategicBomber:
						longTermEffect = StrategicBomber.LongTermEffect_M;
						shortTermEffect = StrategicBomber.ShortTermEffect_M;
						requiredTime = StrategicBomber.RequiredTime_M;
						instruction = () => new StrategicBomber();
						break;

					case Platform.Types.MissileLauncher:
						longTermEffect = MissileLauncher.LongTermEffect_M;
						shortTermEffect = MissileLauncher.ShortTermEffect_M;
						requiredTime = MissileLauncher.RequiredTime_M;
						instruction = () => new MissileLauncher();
						break;

					case Platform.Types.NuclearSubmarine:
						longTermEffect = NuclearSubmarine.LongTermEffect_M;
						shortTermEffect = NuclearSubmarine.ShortTermEffect_M;
						requiredTime = NuclearSubmarine.RequiredTime_M;
						instruction = () => new NuclearSubmarine();
						break;

					default:
						throw new NotImplementedException();
				}

				manufacture = new Manufacture(name, instruction, longTermEffect, shortTermEffect, requiredTime);
				Data.Me.TaskSequence.AddNewTask(manufacture);
				EventManager.TaskProgressAdvenced += NukeStrikePlatformManufactureCompleted;				
			}
			private static void NukeStrikePlatformManufactureCompleted(Task sender, TaskProgressAdvancedEventArgs e)
			{
				if (e.IsTaskFinished && 
					sender is Manufacture manufacture &&
					manufacture.FinalProduct is Platform platform)
				{
					Data.Me.Reserve.Add("Platform", platform);
				}
			}

			public static void DestroyNuke(NuclearWeapon nuclearWeapon)
			{
				// Further investigation is needed
				Deployment destruction = new Deployment($"Destroying {nuclearWeapon}", null, nuclearWeapon, null, null, null);
				Data.Me.TaskSequence.AddNewTask(destruction);

				EventManager.TaskProgressAdvenced += (s, e) => RemoveNuke(s, e, nuclearWeapon);
			}
			private static void RemoveNuke(Task sender, TaskProgressAdvancedEventArgs e, NuclearWeapon weapon)
			{
				if (e.IsTaskFinished &&
					sender is Deployment deployment && 
					deployment.DeployableObject == weapon)
				{
					weapon.DestroyThis();
				}
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