#r "../../../Core/bin/debug/netcoreapp3.1/Core.dll"
#r "System.Runtime.dll"
#r "System.Collections.dll"

using System;
using System.Collections.Generic;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

string[] DestroyNuke(NuclearWeapon nuclearWeapon)
{
	Deployment destruction = new Deployment($"Destroying {nuclearWeapon}", null, nuclearWeapon,
		ResourceManager.Misc.NukeDisposal);
	ResourceManager.Me.TaskSequence.AddNewTask(destruction);

	EventManager.TaskProgressAdvenced += (s, e) => RemoveNuke(s, e, nuclearWeapon);

	return new string[0];

	void RemoveNuke(Task sender, TaskProgressAdvancedEventArgs e, NuclearWeapon weapon)
	{
		if (e.IsTaskFinished &&
			sender is Deployment deployment &&
			deployment.DeployableObject == weapon)
		{
			weapon.DestroyThis();
		}
	}
}

string[] EnrollNukeStrikePlatform(Platform.Types type)
{
	Manufacture manufacture;
	string name = $"Sending a new {type} to reserve";
	CostOfStage cost = ResourceManager.GetCostOf(type.ToString(), TypesOfCostOfStage.Manufacture);
	Func<Platform> onCompleteAction;

	switch (type)
	{
		case Platform.Types.Silo:
			onCompleteAction = () => new Silo();
			break;

		case Platform.Types.StrategicBomber:
			onCompleteAction = () => new StrategicBomber();
			break;

		case Platform.Types.MissileLauncher:
			onCompleteAction = () => new MissileLauncher();
			break;

		case Platform.Types.NuclearSubmarine:
			onCompleteAction = () => new NuclearSubmarine();
			break;

		default:
			throw new NotImplementedException();
	}


	manufacture = new Manufacture(name, onCompleteAction, cost)
	{
		ConfidentialLevel = ConfidentialLevel.Domestic,
		//Influence = Influence.Positive
	};
	ResourceManager.Me.TaskSequence.AddNewTask(manufacture);
	EventManager.TaskProgressAdvenced += NukeStrikePlatformManufactureCompleted;

	return new string[0];

	void NukeStrikePlatformManufactureCompleted(Task sender, TaskProgressAdvancedEventArgs e)
	{
		if (e.IsTaskFinished &&
			sender is Manufacture manufacture &&
			manufacture.FinalProduct is Platform platform)
		{
			ResourceManager.Me.SendToReserve(platform);
		}
	}
}