using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public class GameObjectList<T>
	{
		public event Action<GameObjectList<T>, T, T> OnElementChanged;
		private List<T> list = new List<T>();
		public T this[int index]
		{
			get => list[index];
			set
			{
				OnElementChanged(this, list[index], value);
				list[index] = value;
			}
		}

		public event Action<GameObjectList<T>, T> OnAddItem;
		public void Add(T item)
		{
			list.Add(item);
			OnAddItem?.Invoke(this, item);
		}
	}
}
