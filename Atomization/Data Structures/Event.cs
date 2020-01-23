using Atomization.DataStructures;
using System;

namespace Atomization
{
	public delegate void TaskProgressAdvanceHandler(Task sender, TaskProgressAdvancedEventArgs e);
	public delegate void ValueUpdatedHandler(ValueComplex sender, ValueUpdatedEventArgs e);
	//public delegate void TaskCompletedHandler(Task sender, TaskCompletedEventArgs e);

	public abstract class GameEventArgs : EventArgs
	{

	}

	public class TaskProgressAdvancedEventArgs : GameEventArgs
	{
		public readonly int TimeRemaining;
		public bool IsTaskFinished => TimeRemaining <= 0;
		public TaskProgressAdvancedEventArgs(int timeRemaining)
		{
			TimeRemaining = timeRemaining;
		}
	}

	public class ValueUpdatedEventArgs : GameEventArgs
	{
		
	}
}