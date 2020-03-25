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
			ResourceManager.Initialize();
			//Loader.InitializeCosts();
			GameManager.InitializeEntityAttributes();

			Loader.InitializeMe();
			ResourceManager.Me.MainCommandGroups = Loader.GetAllCommandGroups();
		}
	}
}
