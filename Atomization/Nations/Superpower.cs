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
				RegularNation nation = new RegularNation() { Name = ResourceManager.NationNames.Dequeue() };
				ResourceManager.Regions.Add(nation);
				// initialize the no. of adj nations
				Adjacency[i] = nation;
			}

			Affiliation = this;

		}

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public CommandGroup Nuclear { get; }

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
					ResourceManager.Me.NationalIndices[i].Growth.AddTerm(effectName, effect[i]);
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

		public void SendToReserve(IUniqueObject uniqueObject)
		{
			_Reserve.Add(uniqueObject.UID, uniqueObject);
		}
		public T GetFromReserve<T>(string uid) where T : IUniqueObject
		{
			return (T)_Reserve[uid];
		}

		#region Temp holder for static commands
		//public static void DeployNewNuclearStrikePlatform(Platform.Types type, Region region)
		//{
		//	Data.Me.TaskSequence.AddNewTask(Task.Types.MTD, $"Deploying a new {type}");
		//	throw new NotImplementedException();
		//}

		private static void EnrollNukeStrikePlatfrom(Platform.Types type)
		{
			Manufacture manufacture;
			string name = $"Sending a new {type} to reserve";
			CostOfStage cost;
			Func<Platform> instruction;

			switch (type)
			{
				case Platform.Types.Silo:
					cost = Silo.Manufacture;
					instruction = () => new Silo();
					break;

				case Platform.Types.StrategicBomber:
					cost = StrategicBomber.Manufacture;
					instruction = () => new StrategicBomber();
					break;

				case Platform.Types.MissileLauncher:
					cost = MissileLauncher.Manufacture;
					instruction = () => new MissileLauncher();
					break;

				case Platform.Types.NuclearSubmarine:
					cost = NuclearSubmarine.Manufacture;
					instruction = () => new NuclearSubmarine();
					break;

				default:
					throw new NotImplementedException();
			}

			manufacture = new Manufacture(name, instruction, cost);
			ResourceManager.Me.TaskSequence.AddNewTask(manufacture);
			EventManager.TaskProgressAdvenced += NukeStrikePlatformManufactureCompleted;
		}
		private static void NukeStrikePlatformManufactureCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished &&
				sender is Manufacture manufacture &&
				manufacture.FinalProduct is Platform platform)
			{
				ResourceManager.Me.SendToReserve(platform);
			}
		}

		private static void DisposeNuke(NuclearWeapon nuclearWeapon)
		{
			Deployment destruction = new Deployment($"Destroying {nuclearWeapon}", null, nuclearWeapon,
				ResourceManager.Misc.NukeDisposal);
			ResourceManager.Me.TaskSequence.AddNewTask(destruction);

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

		private static void InvadeAndStation(Nation target, double forcesFromMe, double forcesFromAlliance,
			bool doSupportaPuppet = false) { }

		public static void Assinate(Nation target, PublicFigures figure) { }
		#endregion

		public enum PublicFigures
		{

		}

	}
}