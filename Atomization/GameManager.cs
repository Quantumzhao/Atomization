using Atomization.DataStructures;
using System;

namespace Atomization
{
	public static class GameManager
	{
		public static event Action TimeElapsed;

		public static void InitializeAll()
		{
			Platform.InitializeStaticMember();
			ResourceManager.Misc.Initialize();
		}

		private static ulong _Counter = 0;
		public static string GenerateUID() => _Counter++.ToString("X");
	}
}