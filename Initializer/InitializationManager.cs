using LCGuidebook.Core;

namespace LCGuidebook.Initializer.Manager
{
	public static class InitializationManager
	{
		public static void InitializeAll()
		{
			Loader.
			Loader.InitializeMap();
			//ResourceManager.Initialize();
			Loader.InitializeCosts();

			Loader.InitializeMe();
			ResourceManager.Me.MainCommandGroups = Loader.GetAllActionGroups();
		}
	}
}
