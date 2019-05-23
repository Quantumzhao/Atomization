using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public delegate void OnValueChanged<in S, T>(S sender, T previousValue, T newValue);
	public delegate void OnValueAdded<T>(IEnumerable<T> sender, T addedItem);
	public delegate void OnValueDeleted<T>(IEnumerable<T> sender, T deletedItem);
	public class GameObjectList<T> : List<T>
	{
		public new int Capacity { get; set; } = 0;
		public bool IsLimitedCapacity { get; set; } = false;

		public event Action<GameObjectList<T>, T, T> OnItemChanged;

		public event Action<GameObjectList<T>, T> OnItemAdded;

		public event Action<GameObjectList<T>, T> OnItemRemoved;

		public new bool Add(T item)
		{
			if (Count >= Capacity && IsLimitedCapacity)
			{
				return false;
			}
			else
			{
				base.Add(item);
				OnItemAdded?.Invoke(this, item);

				return false;
			}
		}

		public new bool Remove(T item)
		{
			OnItemRemoved?.Invoke(this, item);
			return base.Remove(item);
		}
	}
}
