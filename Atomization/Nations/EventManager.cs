﻿using Atomization.DataStructures;
using System;

namespace Atomization
{
	public static class EventManager
	{
		public static event TaskProgressAdvanceHandler TaskProgressAdvenced;
		public static event ValueUpdatedHandler ValueUpdated;
		//public static event TaskCompletedHandler TaskCompleted;

		public static void RaiseEvent(object sender, GameEventArgs e)
		{
			if (e is TaskProgressAdvancedEventArgs se)
			{
				TaskProgressAdvenced?.Invoke(sender as Task, se);
			}
			else if (e is ValueUpdatedEventArgs ve)
			{
				ValueUpdated?.Invoke(sender as ValueComplex, ve);
			}
		}
	}

	public static class GameManager
	{
		public static event Action TimeElapsed;
	}
}