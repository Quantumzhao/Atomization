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
		private Task() { }
		public static Task Create()
		{
			throw new NotImplementedException();
		}

		public event StageFinishedHandler StageFinished;

		public string Name { get; set; }
		public string UID { get; set; }

		public void RemoveCurrentStage()
		{
			var stage = this.Dequeue();
			StageFinished?.Invoke(stage, this.Peek(), this);
		}
	}
}
 