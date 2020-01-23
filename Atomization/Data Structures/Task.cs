/* Obsolete */
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

namespace Atomization.DataStructures
{
	// This class is only for `TaskSequence` property of `Nation` class
	public class TaskSequence : INotifyCollectionChanged
	{
		public TaskSequence()
		{
			EventManager.TaskProgressAdvenced += OnTaskCompleted;
		}

		private readonly List<Task> _Tasks = new List<Task>();

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public void AddNewTask(Task.Types task, string name, Action<Task> onExecute)
		{
			switch (task)
			{
				case Task.Types.Census:
					var census = new Census();
					census.DoCensus = onExecute;
					_Tasks.Add(census);
					break;

				case Task.Types.Policy:
					break;

				case Task.Types.Manufacture:
					break;

				case Task.Types.Transportation:
					break;

				case Task.Types.Deployment:
					break;

				case Task.Types.MT:
					break;

				case Task.Types.TD:
					break;

				case Task.Types.MTD:
					break;

				default:
					break;
			}
			//_Tasks.Add(newTask);
		}

		private void OnTaskCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished)
			{
				_Tasks.Remove(sender);
			}
		}
	}

	/* A procedure is a set of tasks. 
	 * e.g. an operation of deploying a new set of armament 
	 * involves purchasing, manufacture, transportation and finally deployment. 
	 * Each stage itself requires time and resource. 
	 * When a certain stage is finished, the next stage begins. */
	/* public class Task : Queue<Stage>
	{
		private Task(string name) 
		{
			EventManager.StageProgressAdvenced += OnStageProgressAdvanced;

			Name = name;
		}

		public static Task Create(Types type, Action onExecute, string name)
		{
			switch (type)
			{
				case Types.Census:
					return CreateCensus(Census.Create(-1), name);
				case Types.Policy:
					break;
				case Types.Manufacture:
					break;
				case Types.Transportation:
					break;
				case Types.Deployment:
					break;
				case Types.MT:
					break;
				case Types.TD:
					break;
				case Types.MTD:
					break;
				default:
					break;
			}
			throw new NotImplementedException();
		}

		public string Name { get; set; }

		private static Task CreateCensus(Census census, string name)
		{
			Task task = new Task(name);

			task.Enqueue(census);

			return task;
		}

		private void OnStageProgressAdvanced(Stage sender, StageProgressAdvancedEventArgs e)
		{
			if (sender.Equals(this.Peek()) && e.IsStageFinished)
			{
				sender.Execute();
				this.Dequeue();

				if (this.Count == 0)
				{
					EventManager.RaiseEvent(this, new TaskCompletedEventArgs());
				}
			}
		}

	}*/
}
 