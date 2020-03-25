using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System;
using System.Collections.Generic;

namespace LCGuidebook.Core
{
	public static class GameManager
	{
		public static event Action TimeElapsed;

		private static ulong _Counter = 0;
		public static string GenerateUID() => _Counter++.ToString("X");

		public static void TimeElapse() => TimeElapsed?.Invoke();
	}

	public static class EventVisualizer
	{
		public static List<GameEventArgs> Events = new List<GameEventArgs>();

		public static void Initialize()
		{
			GameManager.TimeElapsed += GameManager_TimeElapsed;
			EventManager.TaskProgressAdvenced += EventManager_TaskProgressAdvenced;

			static void GameManager_TimeElapsed()
			{
				Events.Clear();
			}
			static void EventManager_TaskProgressAdvenced(Task sender, TaskProgressAdvancedEventArgs e)
			{
				Events.Add(e);
			}
		}
	}
}