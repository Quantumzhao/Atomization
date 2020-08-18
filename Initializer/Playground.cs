using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace LCGuidebook.Initializer
{
	public class AsmField : AbstractAction, IActionGroupField
	{
		public AsmField(ActionGroup parent) : base(parent) { }

		public object Value { get; set; }
	}

	public class AsmExecution : AbstractAction, IActionGroupExecution
	{
		public AsmExecution(ActionGroup parent, MethodInfo body) : base(parent) => Body = body;

		public MethodInfo Body { get; set; }
		public object[] Arguments { get; set; } = Array.Empty<object>();

		public void Execute()
		{
			Body.Invoke(null, Arguments);
		}
	}

	public class AssemblyCodeLoader
	{
		public static ActionGroup BuildActionGroup()
		{
			var ret = new ActionGroup("Test");

			foreach (var method in typeof(Playground).GetMethods(BindingFlags.Public & BindingFlags.Static))
			{
				ret.Actions.Add(new AsmExecution(ret, method));
			}

			return ret;
		}

	}

	public static class Playground
	{

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

		//#region EnrollNewNuclearStrikeForce
		//// This is a simplified process of manufacture of nuclear arsenal, 
		//// with all the details being intentionally ignored
		//static void EnrollNewNuclearStrikeForce()
		//{
		//	CostOfStage carrierCost(string enumName) 
		//		=> ResourceManager.GetCostOf(enumName, TypesOfCostOfStage.Manufacture);
		//	string name(string enumName) => $"Sending a new {enumName} to reserve";
		//	Manufacture Instantiate(Func<IDestroyable> product, string enumName) 
		//		=> new Manufacture(name(enumName), product, carrierCost(enumName));

		//	Func<NuclearWeapon> carrierManufactureDef;
		//	switch (Range)
		//	{
		//		case CarrierType.IRBM:
		//			carrierManufactureDef = () => new IRBM();
		//			break;

		//		case CarrierType.ICBM:
		//			carrierManufactureDef = () => new ICBM();
		//			break;

		//		case CarrierType.AerialBomb:
		//			carrierManufactureDef = () => new NuclearBomb();
		//			break;

		//		default:
		//			throw new InvalidOperationException();
		//	}
		//	var carrierManufacture = Instantiate(carrierManufactureDef, Range.ToString());
		//	var carrierManufactureUid = carrierManufacture.UID;
		//	ResourceManager.Me.TaskSequence.AddNewTask(carrierManufacture);
		//	EventManager.TaskProgressAdvenced += OnCarrierCompleted;

		//	Func<Warhead> warheadManufactureDef;
		//	switch (Power)
		//	{
		//		case Warhead.Types.AtomicBomb:
		//			warheadManufactureDef = () => new Atomic();
		//			break;

		//		case Warhead.Types.HydrogenBomb:
		//			warheadManufactureDef = () => new Hydrogen();
		//			break;

		//		case Warhead.Types.DirtyBomb:
		//			warheadManufactureDef = () => new Dirty();
		//			break;

		//		default:
		//			throw new InvalidOperationException();
		//	}
		//	var warheadManufacture = Instantiate(warheadManufactureDef, Range.ToString());
		//	var warheadManufactureUid = warheadManufacture.UID;
		//	ResourceManager.Me.TaskSequence.AddNewTask(warheadManufacture);
		//	EventManager.TaskProgressAdvenced += OnWarheadCompleted;

		//	Func<Platform> platformManufactureDef;
		//	switch (Concealment)
		//	{
		//		case Platform.Types.Silo:
		//			platformManufactureDef = () => new Silo();
		//			break;

		//		case Platform.Types.StrategicBomber:
		//			platformManufactureDef = () => new StrategicBomber();
		//			break;

		//		case Platform.Types.MissileLauncher:
		//			platformManufactureDef = () => new MissileLauncher();
		//			break;

		//		case Platform.Types.NuclearSubmarine:
		//			platformManufactureDef = () => new NuclearSubmarine();
		//			break;

		//		default:
		//			throw new InvalidOperationException();
		//	}
		//	var platformManufacture = Instantiate(platformManufactureDef, Range.ToString());
		//	var platformManufactureUid = platformManufacture.UID;
		//	ResourceManager.Me.TaskSequence.AddNewTask(platformManufacture);
		//	EventManager.TaskProgressAdvenced += OnPlatformCompleted;

		//	Manufacture finalProductManufacture = null;
		//	Func<Platform> Assemble = () =>
		//	{
		//		Platform finalProduct = null;
		//		NuclearWeapon nuclearWeapon = null;
		//		Warhead warhead = null;
		//		foreach (Manufacture task in finalProductManufacture.Dependence)
		//		{
		//			if (task.UID == platformManufactureUid)
		//			{
		//				finalProduct = task.FinalProduct as Platform;
		//			}
		//			else if (task.UID == warheadManufactureUid)
		//			{
		//				warhead = task.FinalProduct as Warhead;
		//			}
		//			else if (task.UID == carrierManufactureUid)
		//			{
		//				nuclearWeapon = task.FinalProduct as NuclearWeapon;
		//			}
		//			else throw new InvalidOperationException();
		//		}
		//		// Just for testing, will change
		//		nuclearWeapon.Platform = finalProduct;
		//		nuclearWeapon.Warheads.Add(warhead);
		//		finalProduct.NuclearWeapons.Add(nuclearWeapon);
		//		return finalProduct;
		//	};

		//	finalProductManufacture = new Manufacture("Final Product", Assemble, 
		//		new CostOfStage(Effect.GenerateEmptyEffect(), Effect.GenerateEmptyEffect(), (Expression)0));
		//	EventManager.TaskProgressAdvenced += OnFinalProductCompleted;

		//	throw new NotImplementedException();
		//}
		//static void OnCarrierCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//{
		//	if (e.IsTaskFinished &&
		//		sender is Manufacture manufacture &&
		//		manufacture.FinalProduct is NuclearWeapon carrier)
		//	{
		//		ResourceManager.Me.SendToReserve(carrier);
		//	}
		//}
		//static void OnWarheadCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//{
		//	if (e.IsTaskFinished &&
		//		sender is Manufacture manufacture &&
		//		manufacture.FinalProduct is Warhead warhead)
		//	{
		//		ResourceManager.Me.SendToReserve(warhead);
		//	}
		//}
		//static void OnPlatformCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//{
		//	if (e.IsTaskFinished &&
		//		sender is Manufacture manufacture &&
		//		manufacture.FinalProduct is Platform platform)
		//	{
		//		ResourceManager.Me.SendToReserve(platform);
		//	}
		//}
		//static void OnFinalProductCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//{
		//	if (e.IsTaskFinished &&
		//		sender is Manufacture manufacture &&
		//		manufacture.FinalProduct is Platform platform)
		//	{
		//		ResourceManager.Me.SendToReserve(platform);
		//	}
		//}

		//static string NO_SUCH_PROPERTY(string name) 
		//	=> $"The property \"{name}\" does not exist in the script";

		//static public CostOfStage CurrentCost { get; private set; } 
		//	= new CostOfStage(Effect.GenerateEmptyEffect(), Effect.GenerateEmptyEffect(), (Expression)0);

		//static public CarrierType Range { get; private set; }

		//static public Warhead.Types Power { get; private set; }

		//static public Platform.Types Concealment { get; private set; }

		//static void UpdateCost()
		//{
		//	CurrentCost 
		//		= ResourceManager.GetCostOf(Range.ToString(), TypesOfCostOfStage.Manufacture)
		//		+ ResourceManager.GetCostOf(Power.ToString(), TypesOfCostOfStage.Manufacture)
		//		+ ResourceManager.GetCostOf(Concealment.ToString(), TypesOfCostOfStage.Manufacture);
		//}

		////static public string[] Set(string propertyName, object value)
		////{
		////	switch (propertyName)
		////	{
		////		case nameof(Range):
		////			Range = (CarrierType)value;
		////			break;

		////		case nameof(Power):
		////			Power = (Warhead.Types)value;
		////			break;

		////		case nameof(Concealment):
		////			Concealment = (Platform.Types)value;
		////			break;

		////		default:
		////			throw new MemberAccessException(NO_SUCH_PROPERTY(propertyName));
		////	}

		////	UpdateCost();
		////	return new string[] { nameof(CurrentCost) };
		////}
		//#endregion

		//#region DeployNewInstallation
		//string[] DeployNewInstallation(Region deployedRegion)
		//{
		//	Manufacture manufacture;
		//	Deployment deployment;

		//	Func<IDeployable> onCompletion = () => new Installation();

		//	manufacture = new Manufacture(
		//		"Sending a new missle defense system to reserve", onCompletion, 
		//		ResourceManager.GetCostOf(nameof(Installation), TypesOfCostOfStage.Manufacture)
		//	);
		//	ResourceManager.Me.TaskSequence.AddNewTask(manufacture);
		//	EventManager.TaskProgressAdvenced += NewInstallationManufactureCompleted;

		//	deployment = new Deployment($"Deploying a new missile defense system in {deployedRegion}", 
		//		deployedRegion, (IDeployable)manufacture.FinalProduct, 
		//		ResourceManager.GetCostOf(nameof(Installation), TypesOfCostOfStage.Deployment));
		//	ResourceManager.Me.TaskSequence.AddNewTask(deployment);
		//	EventManager.TaskProgressAdvenced += NewDeploymentCompleted;

		//	return new string[0];

		//	static void NewDeploymentCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//	{
		//		if (e.IsTaskFinished &&
		//			sender is Deployment deployment &&
		//			deployment.DeployableObject.UID == e.RelatedGameObjectUid)
		//		{
		//			ResourceManager.Me.GetFromReserve<Installation>(e.RelatedGameObjectUid).DeployedRegion
		//				= deployment.Destination;
		//		}
		//	}

		//	static void NewInstallationManufactureCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		//	{
		//		if (e.IsTaskFinished &&
		//			sender is Manufacture manufacture &&
		//			manufacture.FinalProduct.UID == e.RelatedGameObjectUid)
		//		{
		//			ResourceManager.Me.SendToReserve(manufacture.FinalProduct);
		//		}
		//	}
		//}
		////string[] Set(string propertyName, object value)
		////{
		////	switch (propertyName)
		////	{
		////		case nameof(DeployedRegion):
		////			DeployedRegion = (Region)value;
		////			break;

		////		default:
		////			throw new InvalidOperationException();
		////	}

		////	return new string[] { nameof(DeployedRegion) };
		////}

		//string[] RelocateInstallation(Installation installation, Region destination)
		//{
		//	Transportation transportation;
		//	transportation = new Transportation($"Transferring {installation} to a new destination {destination}", installation, null, 
		//		ResourceManager.GetCostOf(nameof(Installation), TypesOfCostOfStage.Transportation));

		//	return new string[0];
		//}
		//#endregion
		#endregion

		#region TestPack
		public static string[] LaunchNuke(Nation from, Nation to)
		{
			var name = "Delivering payload";
			var missile = (ResourceManager.Me.MiscProperties["NukeArsenal"] as Queue<NuclearMissile>).Dequeue();
			missile.Target = to;
			var transportations = Transportation.Create(name, missile, from, to,
				ResourceManager.GetCostOf(nameof(NuclearMissile), TypesOfCostOfStage.Transportation));
			transportations.ForEach(t => ResourceManager.Me.TaskSequence.AddNewTask(t));

			return new string[0];
		}

		public static string[] BuildNuke(Nation nation)
		{
			Manufacture manufacture;

			var name = "Building a missile";
			var cost = ResourceManager.GetCostOf(nameof(NuclearMissile), TypesOfCostOfStage.Manufacture);
			Func<IDestroyable> onCompletion = () => new NuclearMissile(nation);
			Action<Manufacture> afterCompleted = 
				m => (((m.FinalProduct as NuclearMissile).DeployedRegion as Nation)
				.MiscProperties["NukeArsenal"] as Queue<NuclearMissile>).Enqueue(m.FinalProduct as NuclearMissile);

			manufacture = new Manufacture(name, onCompletion, cost);
			ResourceManager.Me.TaskSequence.AddNewTask(manufacture);

			return new string[0];
		}

		public static string[] DestroyNuke(Nation nation)
		{
			Deployment deployment;

			var name = "Deconstructing a missile";
			var cost = ResourceManager.GetCostOf(nameof(NuclearMissile), TypesOfCostOfStage.Deployment);

			deployment = new Deployment(name, null,
				(nation.MiscProperties["NukeArsenal"] as Queue<NuclearMissile>).Dequeue(), cost);
			ResourceManager.Me.TaskSequence.AddNewTask(deployment);

			return new string[0];
		}

		public static string[] Expand(Nation from, Nation to)
		{
			Policy policy;

			var name = $"Making {to} an alliance";
			CostOfStage cost =
				new CostOfStage(Effect.GenerateEmptyEffect(), Effect.GenerateEmptyEffect(), (Expression)8);
			Action action = () =>
			{
				from.Inclination[ResourceManager.Me] = 1;
				ResourceManager.Me.Inclination[from] = 1;
				to.Figures[0].CurrentValue = 50;
			};

			policy = new Policy(name, cost, action);
			ResourceManager.Me.TaskSequence.AddNewTask(policy);

			return new string[0];
		}

		public static string[] TryDefend(Nation nation)
		{
			return new string[0];
		}

		#endregion
	}
}
