#r "../../bin/debug/netcoreapp3.1/Initializer.dll"
#r "../../../Core/bin/debug/netcoreapp3.1/Core.dll"

using System;
using System.Collections.Generic;
using System.Text;
using LCGuidebook.Initialization;

public void TestCommand(int arbitraryNumbers, Context globals)
{
	Console.WriteLine($"Hello world{globals.Me.Defence.Description}");
}