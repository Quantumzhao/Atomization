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

		#region EnrollNewNuclearStrikeForce
		// This is a simplified process of manufacture of nuclear arsenal, 
		// with all the details being intentionally ignored
		static void EnrollNewNuclearStrikeForce()
		{
			CostOfStage carrierCost(string enumName) 
				=> ResourceManager.GetCostOf(enumName, TypesOfCostOfStage.Manufacture);
			string name(string enumName) => $"Sending a new {enumName} to reserve";
			Manufacture Instantiate(Func<IDestroyable> product, string enumName) 
				=> new Manufacture(name(enumName), product, carrierCost(enumName));

			Func<NuclearWeapon> carrierManufactureDef;
			switch (Range)
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
			var carrierManufacture = Instantiate(carrierManufactureDef, Range.ToString());
			var carrierManufactureUid = carrierManufacture.UID;
			ResourceManager.Me.TaskSequence.AddNewTask(carrierManufacture);
			EventManager.TaskProgressAdvenced += OnCarrierCompleted;

			Func<Warhead> warheadManufactureDef;
			switch (Power)
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
			var warheadManufacture = Instantiate(warheadManufactureDef, Range.ToString());
			var warheadManufactureUid = warheadManufacture.UID;
			ResourceManager.Me.TaskSequence.AddNewTask(warheadManufacture);
			EventManager.TaskProgressAdvenced += OnWarheadCompleted;

			Func<Platform> platformManufactureDef;
			switch (Concealment)
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
			var platformManufacture = Instantiate(platformManufactureDef, Range.ToString());
			var platformManufactureUid = platformManufacture.UID;
			ResourceManager.Me.TaskSequence.AddNewTask(platformManufacture);
			EventManager.TaskProgressAdvenced += OnPlatformCompleted;

			Manufacture finalProductManufacture = null;
			Func<Platform> Assemble = () =>
			{
				Platform finalProduct = null;
				NuclearWeapon nuclearWeapon = null;
				Warhead warhead = null;
				foreach (Manufacture task in finalProductManufacture.Dependence)
				{
					if (task.UID == platformManufactureUid)
					{
						finalProduct = task.FinalProduct as Platform;
					}
					else if (task.UID == warheadManufactureUid)
					{
						warhead = task.FinalProduct as Warhead;
					}
					else if (task.UID == carrierManufactureUid)
					{
						nuclearWeapon = task.FinalProduct as NuclearWeapon;
					}
					else throw new InvalidOperationException();
				}
				// Just for testing, will change
				nuclearWeapon.Platform = finalProduct;
				nuclearWeapon.Warheads.Add(warhead);
				finalProduct.NuclearWeapons.Add(nuclearWeapon);
				return finalProduct;
			};

			finalProductManufacture = new Manufacture("Final Product", Assemble, 
				new CostOfStage(new Effect(), new Effect(), (Expression)0));
			EventManager.TaskProgressAdvenced += OnFinalProductCompleted;

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
		static void OnFinalProductCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished &&
				sender is Manufacture manufacture &&
				manufacture.FinalProduct is Platform platform)
			{
				ResourceManager.Me.SendToReserve(platform);
			}
		}

		static string NO_SUCH_PROPERTY(string name) 
			=> $"The property \"{name}\" does not exist in the script";

		static public CostOfStage CurrentCost { get; private set; } 
			= new CostOfStage(new Effect(), new Effect(), (Expression)0);

		static public CarrierType Range { get; private set; }

		static public Warhead.Types Power { get; private set; }

		static public Platform.Types Concealment { get; private set; }

		static void UpdateCost()
		{
			CurrentCost 
				= ResourceManager.GetCostOf(Range.ToString(), TypesOfCostOfStage.Manufacture)
				+ ResourceManager.GetCostOf(Power.ToString(), TypesOfCostOfStage.Manufacture)
				+ ResourceManager.GetCostOf(Concealment.ToString(), TypesOfCostOfStage.Manufacture);
		}

		static public string[] Set(string propertyName, object value)
		{
			switch (propertyName)
			{
				case nameof(Range):
					Range = (CarrierType)value;
					break;

				case nameof(Power):
					Power = (Warhead.Types)value;
					break;

				case nameof(Concealment):
					Concealment = (Platform.Types)value;
					break;

				default:
					throw new MemberAccessException(NO_SUCH_PROPERTY(propertyName));
			}

			UpdateCost();
			return new string[] { nameof(CurrentCost) };
		}
		#endregion

		#region DeployNewInstallation
		void DeployNewInstallation()
		{

		}
		#endregion
		#endregion

		public enum PublicFigures
		{

		}
	}
}