using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using Atomization.DataStructures;

namespace Atomization.DataStructures
{
	public delegate void StageFinishedHandler(Stage previous, Stage next, Task task);

	//public class VMList<V> : List<V>, INotifyCollectionChanged
	//{
	//	public event PropertyChangedEventHandler PropertyChanged;
	//	public event NotifyCollectionChangedEventHandler CollectionChanged;

	//	public new void Add(V newValue)
	//	{
	//		base.Add(newValue);
	//		CollectionChanged?.Invoke(this, 
	//			new NotifyCollectionChangedEventArgs(
	//				NotifyCollectionChangedAction.Add, newValue
	//			)
	//		);
	//	}

	//	public new bool Remove(V oldValue)
	//	{
	//		if (base.Remove(oldValue))
	//		{
	//			CollectionChanged?.Invoke(this,
	//				new NotifyCollectionChangedEventArgs(
	//					NotifyCollectionChangedAction.Remove, oldValue
	//				)
	//			);
	//			return true;
	//		}
	//		else
	//		{
	//			return false;
	//		}
	//	}

	//	//public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
	//	//{
	//	//	CollectionChanged?.Invoke(this, e);
	//	//}
	//}
	public static class Misc
	{
		public static int Round(double value) => (int)(value + 0.5);
	}

	public interface IExecutable
	{
		void Execute();
	}
}
