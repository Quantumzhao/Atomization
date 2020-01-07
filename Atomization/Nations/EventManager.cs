using Atomization.DataStructures;
using System;

namespace Atomization
{
	public static class EventManager
	{
		public static void RaiseEvent(object sender, GameEventArgs e)
		{
			if (e is StageProgressAdvancedEventArgs se)
			{
				StageProgressAdvenced?.Invoke(sender as Stage, se);
			}
		}

		public static event StageProgressAdvanceHandler StageProgressAdvenced;
	}

	public static class GameManager
	{
		public static event Action TimeElapsed;
	}
}