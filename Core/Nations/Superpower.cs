using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.Collections.Generic;
using System;
using UIEngine;
using System.Reflection.PortableExecutable;

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
		}

		[Visible(nameof(MainCommandGroups))]
		public List<ActionGroup> MainCommandGroups { get; set; }

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

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

		public static void DestroyNuke(NuclearWeapon nuclearWeapon)
		{
			Deployment destruction = new Deployment($"Destroying {nuclearWeapon}", null, nuclearWeapon,
				ResourceManager.Misc.NukeDisposal);
			ResourceManager.Me.TaskSequence.AddNewTask(destruction);

			EventManager.TaskProgressAdvenced += (s, e) => RemoveNuke(s, e, nuclearWeapon);
		}
		static void RemoveNuke(Task sender, TaskProgressAdvancedEventArgs e, NuclearWeapon weapon)
		{
			if (e.IsTaskFinished &&
				sender is Deployment deployment &&
				deployment.DeployableObject == weapon)
			{
				weapon.DestroyThis();
			}
		}

		//public static void DeployNewNuclearWeapon(Platform.Types platformType, 
		//	Warhead.Types warheadType, CarrierType carrierType, Region target)
		//{
		//	Manufacture manufacture;
		//	string name = $"Sending a new {warheadType} to reserve";
		//	CostOfStage cost = ResourceManager.GetCostOf(warheadType.ToString(), TypesOfCostOfStage.Manufacture);
		//	Func<Platform> onCompleteAction;

		//	switch (warheadType)
		//	{
		//		case Warhead.Types.AtomicBomb:
					
		//			break;
		//		case Warhead.Types.HydrogenBomb:
		//			break;
		//		case Warhead.Types.DirtyBomb:
		//			break;
		//		default:
		//			throw new ArgumentException();
		//	}

		//	manufacture = new Manufacture(name, onCompleteAction, cost)
		//	{
		//		ConfidentialLevel = ConfidentialLevel.Domestic,
		//		Influence = Influence.Positive
		//	};
		//	ResourceManager.Me.TaskSequence.AddNewTask(manufacture);
		//	EventManager.TaskProgressAdvenced += NukeStrikePlatformManufactureCompleted;

		//}
		//static void NukeStrikePlatformManufactureCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//{
		//	if (e.IsTaskFinished &&
		//		sender is Manufacture manufacture &&
		//		manufacture.FinalProduct is Platform platform)
		//	{
		//		ResourceManager.Me.SendToReserve(platform);
		//	}
		//}

		// This is a simplified process of manufacture of nuclear arsenal, 
		// with all the details being intentionally ignored
		static void EnrollNewNuclearStrikeForce(CarrierType range, Warhead.Types power, Platform.Types concealment)
		{
			Manufacture overall;
			Manufacture carrier;
			CostOfStage carrierCost(string enumName) 
				=> ResourceManager.GetCostOf(enumName, TypesOfCostOfStage.Manufacture);
			string name(string enumName) => $"Sending a new {enumName} to reserve";
			Manufacture Instantiate(Func<IDestroyable> product, string enumName) 
				=> new Manufacture(name(enumName), product, carrierCost(enumName));

			Func<NuclearWeapon> carrierManufactureDef;
			switch (range)
			{
				case CarrierType.IRBM:
					carrierManufactureDef = () => new IRBM();
					break;

				case CarrierType.ICBM:
					carrierManufactureDef = () => new ICBM();
					break;

				case CarrierType.AerialBomb:
					carrierManufactureDef = () => new NuclearBomb();
					break;

				default:
					throw new InvalidOperationException();
			}
			ResourceManager.Me.TaskSequence.AddNewTask(Instantiate(carrierManufactureDef, range.ToString()));
			EventManager.TaskProgressAdvenced += OnCarrierCompleted;

			Func<Warhead> warheadManufactureDef;
			switch (power)
			{
				case Warhead.Types.AtomicBomb:
					warheadManufactureDef = () => new Atomic();
					break;

				case Warhead.Types.HydrogenBomb:
					warheadManufactureDef = () => new Hydrogen();
					break;

				case Warhead.Types.DirtyBomb:
					warheadManufactureDef = () => new Dirty();
					break;

				default:
					throw new InvalidOperationException();
			}
			ResourceManager.Me.TaskSequence.AddNewTask(Instantiate(warheadManufactureDef, range.ToString()));
			EventManager.TaskProgressAdvenced += OnWarheadCompleted;

			Func<Platform> platformManufactureDef;
			switch (concealment)
			{
				case Platform.Types.Silo:
					platformManufactureDef = () => new Silo();
					break;

				case Platform.Types.StrategicBomber:
					platformManufactureDef = () => new StrategicBomber();
					break;

				case Platform.Types.MissileLauncher:
					platformManufactureDef = () => new MissileLauncher();
					break;

				case Platform.Types.NuclearSubmarine:
					platformManufactureDef = () => new NuclearSubmarine();
					break;

				default:
					throw new InvalidOperationException();
			}
			ResourceManager.Me.TaskSequence.AddNewTask(Instantiate(platformManufactureDef, range.ToString()));
			EventManager.TaskProgressAdvenced += OnPlatformCompleted;

			throw new NotImplementedException();
		}
		static void OnCarrierCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished &&
				sender is Manufacture manufacture &&
				manufacture.FinalProduct is NuclearWeapon carrier)
			{
				ResourceManager.Me.SendToReserve(carrier);
			}
		}
		static void OnWarheadCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished &&
				sender is Manufacture manufacture &&
				manufacture.FinalProduct is Warhead warhead)
			{
				ResourceManager.Me.SendToReserve(warhead);
			}
		}
		static void OnPlatformCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished &&
				sender is Manufacture manufacture &&
				manufacture.FinalProduct is Platform platform)
			{
				ResourceManager.Me.SendToReserve(platform);
			}
		}

		static void PopularizeEducation()
		{

		}
		#endregion

		public enum PublicFigures
		{

		}
	}
}