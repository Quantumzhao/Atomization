using Atomization.DataStructures;
using System;

namespace Atomization
{
	public delegate void StageProgressAdvanceHandler(Stage sender, StageProgressAdvancedEventArgs e);
	public delegate void ValueUpdatedHandler(ValueComplex sender, ValueUpdatedEventArgs e);

	public abstract class GameEventArgs : EventArgs
	{

	}

	public class StageProgressAdvancedEventArgs : GameEventArgs
	{
		public readonly int TimeRemaining;
		public bool IsStageFinished => TimeRemaining <= 0;
		public StageProgressAdvancedEventArgs(int timeRemaining)
		{
			TimeRemaining = timeRemaining;
		}
	}

	public class ValueUpdatedEventArgs : GameEventArgs
	{
		
	}
}