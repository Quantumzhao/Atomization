using Atomization.DataStructures;
using System;

namespace Atomization
{
	public static class EventManager
	{
		public static event StageProgressAdvanceHandler StageProgressAdvenced;
		public static event ValueUpdatedHandler ValueUpdated;
		public static event TaskCompletedHandler TaskCompleted;

		public static void RaiseEvent(object sender, GameEventArgs e)
		{
			if (e is StageProgressAdvancedEventArgs se)
			{
				StageProgressAdvenced?.Invoke(sender as Stage, se);
			}
			else if (e is ValueUpdatedEventArgs ve)
			{
				ValueUpdated?.Invoke(sender as ValueComplex, ve);
			}
			else if (e is TaskCompletedEventArgs)
			{
				
			}
		}
	}

	public static class GameManager
	{
		public static event Action TimeElapsed;
	}
}