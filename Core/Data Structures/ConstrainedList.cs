using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

namespace LCGuidebook.Core.DataStructures
{
	public class ConstrainedList<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		public int MaxCapacity { get; set; } = int.MaxValue;
		public bool IsLimitedCapacity => MaxCapacity != int.MaxValue;

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public new bool Add(T item)
		{
			if (Count >= MaxCapacity)
			{
				return false;
			}
			else
			{
				base.Add(item);
				CollectionChanged?.Invoke(
					this,
					new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item)
				);
				return false;
			}
		}

		public new bool Remove(T item)
		{
			if (base.Remove(item))
			{
				CollectionChanged?.Invoke(
					this,
					new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
				);
				return true;
			}
			else
			{
				return false;
			}
		}

		//public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		//{
		//	CollectionChanged?.Invoke(this, e);
		//}
	}
}