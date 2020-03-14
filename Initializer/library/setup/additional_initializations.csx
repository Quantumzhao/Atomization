#r "../../../Core/bin/debug/netcoreapp3.1/Core.dll"
#r "System.Runtime.dll"
#r "System.Collections.dll"

using System;
using System.Collections.Generic;
using System.Text;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

public void InitNukeArsenal()
{
	for (int i = 0; i < Superpower.INITIAL_NUKE_SILOS; i++)
	{
		ResourceManager.Me.NuclearPlatforms.Add(new Silo());
	}
}