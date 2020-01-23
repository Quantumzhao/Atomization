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

		public void AddNewTask(Task task) => _Tasks.Add(task);

		//public void AddNewTask(Task.Types task, string name, Action<Task> onExecute)
		//{
		//	switch (task)
		//	{
		//		case Task.Types.Census:
		//			var census = new Census(name);
		//			census.DoCensus = onExecute;
		//			_Tasks.Add(census);
		//			break;

		//		case Task.Types.Policy:
		//			break;

		//		case Task.Types.Manufacture:
		//			var manu = new Manufacture(name);

		//			break;

		//		case Task.Types.Transportation:
		//			break;

		//		case Task.Types.Deployment:
		//			break;

		//		case Task.Types.MT:
		//			break;

		//		case Task.Types.TD:
		//			var transportation = new Transportation(name);
		//			EventManager.TaskProgressAdvenced += ContinueDeployment;
		//			_Tasks.Add(transportation);
		//			break;

		//		case Task.Types.MTD:
		//			break;

		//		default:
		//			break;
		//	}
		//	//_Tasks.Add(newTask);
		//}

		private void OnTaskCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished)
			{
				_Tasks.Remove(sender);
			}
		}

		//private void ContinueDeployment(Task sender, TaskProgressAdvancedEventArgs e)
		//{
		//	if (e.IsTaskFinished &&
		//		sender is Transportation transportation)
		//	{
		//		AddNewTask(Task.Types.Transportation, sender.Name, null);
		//	}
		//}
	}
}
 