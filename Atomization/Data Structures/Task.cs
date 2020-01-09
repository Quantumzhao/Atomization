using System.Collections.Generic;
using System.Collections.Specialized;
using System;

namespace Atomization.DataStructures
{
	// This class is only for `TaskSequence` property of `Nation` class
	public class TaskSequence : INotifyCollectionChanged
	{
		public readonly List<Task> Tasks = new List<Task>();

		public event NotifyCollectionChangedEventHandler CollectionChanged;
	}

	/* A procedure is a set of tasks. 
	 * e.g. an operation of deploying a new set of armament 
	 * involves purchasing, manufacture, transportation and finally deployment. 
	 * Each stage itself requires time and resource. 
	 * When a certain stage is finished, the next stage begins. */
	public class Task : Queue<Stage>, IUniqueObject
	{
		private Task() 
		{
			EventManager.StageProgressAdvenced += OnStageProgressAdvanced;
		}
		public static Task Create(Stage finalStage)
		{
			if (finalStage is Census)
			{
				return CreateCensus(finalStage as Census);
			}
			else if (finalStage is Policy)
			{

			}
			else if (finalStage is Purchase)
			{

			}
			else if (finalStage is Manufacture)
			{

			}
			else if (finalStage is Transportation)
			{

			}
			else if (finalStage is Deployment)
			{

			}
			throw new NotImplementedException();
		}

		public string Name { get; set; }
		public string UID { get; set; }

		public void OnStageProgressAdvanced(Stage sender, StageProgressAdvancedEventArgs e)
		{
			if (sender.Equals(this.Peek()) && e.IsStageFinished)
			{
				sender.Execute();
				this.Dequeue();
			}
		}

		private static Task CreateCensus(Census census)
		{
			Task task = new Task();



			throw new NotImplementedException();
		}
	}
}
 