using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System;

namespace LCGuidebook.Core
{
	public static class GameManager
	{
		public static event Action TimeElapsed;

		public static void InitializeAll()
		{
			ResourceManager.Initialize();
			ResourceManager.Misc.Initialize();
			Platform.InitializeStaticMember();
		}

		private static ulong _Counter = 0;
		public static string GenerateUID() => _Counter++.ToString("X");

		public static void TimeElapse() => TimeElapsed?.Invoke();
	}
}