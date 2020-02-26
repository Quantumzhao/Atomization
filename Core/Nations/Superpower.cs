using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.Collections.Generic;
using System;

namespace LCGuidebook.Core
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

			var cmd = new Command("", "");
			cmd.Body = new Action<Platform.Types>(platform => EnrollNukeStrikePlatfrom(platform));
		}

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public CommandGroup Nuclear { get; }

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

		// set public temporarily for unit tests
		public static void EnrollNukeStrikePlatfrom(Platform.Types type)
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