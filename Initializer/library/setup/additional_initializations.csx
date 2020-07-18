#r "../../../Core/bin/debug/netcoreapp3.1/Core.dll"
#r "System.Runtime.dll"
#r "System.Collections.dll"

using System;
using System.Collections.Generic;
using System.Text;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

public void InitNationalProperties()
{
	ResourceManager.Me.MiscProperties.Add("NukeArsenal", new Queue<NuclearMissile>());
}