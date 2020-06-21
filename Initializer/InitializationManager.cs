using System;
using System.Collections.Generic;
using System.Text;
using LCGuidebook.Core;

namespace LCGuidebook.Initializer.Manager
{
	public static class InitializationManager
	{
		public static void InitializeAll()
		{
			Loader.InitializeMap();
			//ResourceManager.Initialize();
			Loader.InitializeCosts();

			Loader.InitializeMe();
			ResourceManager.Me.MainCommandGroups = Loader.GetAllActionGroups();
		}
	}
}
