using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Atomization.DataStructures
{
	public class Growth : INotifyPropertyChanged
	{
		public Dictionary<string, Expression> Items { get; } = new Dictionary<string, Expression>();

		public double this[string name] => Items[name].Value;

		public event PropertyChangedEventHandler PropertyChanged;

		public bool Contains(string name) => Items.ContainsKey(name);
		public bool Contains(double value) => Items.ContainsValue(value);

		public void AddTerm(string name, Expression expression)
		{
			if (Contains(name))
			{
				Items[name].Add(expression);
			}
			else
			{
				Items.Add(name, expression);
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
		}

		public int Sum
		{
			get
			{
				double sum = 0;
				foreach (var item in Items)
				{
					sum += item.Value.Value;
				}
				return (int)sum;
			}
		}
	}
}