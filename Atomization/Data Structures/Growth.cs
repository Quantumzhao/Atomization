using System.Collections.Generic;
using System.ComponentModel;

namespace Atomization.DataStructures
{
	public class Growth : INotifyPropertyChanged
	{
		public Dictionary<string, double> Items { get; set; } = new Dictionary<string, double>();

		public double this[string name] => Items[name];

		public event PropertyChangedEventHandler PropertyChanged;

		public bool Contains(string name) => Items.ContainsKey(name);
		public bool Contains(double value) => Items.ContainsValue(value);

		public void AddValue(string name, double number)
		{
			if (Contains(name))
			{
				Items[name] += number;
			}
			else
			{
				Items.Add(name, number);
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
					sum += item.Value;
				}
				return (int)sum;
			}
		}
	}
}