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
}

void RemoveNuke(Task sender, TaskProgressAdvancedEventArgs e, NuclearWeapon weapon)
{
	if (e.IsTaskFinished &&
		sender is Deployment deployment &&
		deployment.DeployableObject == weapon)
	{
		weapon.DestroyThis();
	}
}

string[] Set(string propertyName, object value) => new string[0];