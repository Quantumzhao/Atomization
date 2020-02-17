/* Obsolete */
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

namespace LCGuidebook.Core.DataStructures
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

		public void AddNewTask(Task task)
		{
			_Tasks.Add(task);
		}

		private void OnTaskCompleted(Task sender, TaskProgressAdvancedEventArgs e)
		{
			if (e.IsTaskFinished)
			{
				_Tasks.Remove(sender);
			}
		}
	}
}
 