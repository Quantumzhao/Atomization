using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;

namespace Atomization
{
	public delegate void ConstructionCompletedHandler(ConstructableObject deployableObject);

	public class Growth
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
	public class Cost
	{
		public Cost(
			string name,
			Expression economy = null,
			Expression hiEduPopu = null,
			Expression army = null,
			Expression navy = null,
			Expression food = null,
			Expression rawMaterial = null,
			Expression nuclearMaterial = null,
			Expression stability = null
		)
		{
			Name = name;

			// the number of properties
			Values = new GameObjectList<Expression>();
			for (int i = 0; i < 8; i++)
			{
				Values.Add(null);
			}

			Values[0] = economy;
			Values[1] = hiEduPopu;
			Values[2] = army;
			Values[3] = navy;
			Values[4] = food;
			Values[5] = rawMaterial;
			Values[6] = nuclearMaterial;
			Values[7] = stability;

			for (int i = 0; i < 8; i++)
			{
				if (Values[i] == null)
				{
					Values[i] = new Expression(0);
				}
			}
		}
		public Cost(
			string name, 
			double economy = 0,
			double hiEduPopu = 0,
			double army = 0,
			double navy = 0,
			double food = 0,
			double rawMaterial = 0,
			double nuclearMaterial = 0,
			double stability = 0
		) : this
		(
			name,
			new Expression(economy),
			new Expression(hiEduPopu),
			new Expression(army),
			new Expression(navy),
			new Expression(food),
			new Expression(rawMaterial),
			new Expression(nuclearMaterial),
			new Expression(stability)
		) { }

		public readonly GameObjectList<Expression> Values;

		public event PropertyChangedEventHandler PropertyChanged;

		public string Name { get; set; }

		public double Economy => Values[0].Value;

		public double HiEduPopu => Values[1].Value;

		public double Army => Values[2].Value;

		public double Navy => Values[3].Value;

		public double Food => Values[4].Value;

		public double RawMaterial => Values[5].Value;

		public double NuclearMaterial => Values[6].Value;

		public double Stability => Values[7].Value;
	}

	public class Expression : INotifyPropertyChanged
	{
		public Expression(double para, Func<double, double> function)
		{
			Parameter = para;
			this._Function = function;
		}
		public Expression(double value)
		{
			_Function = p => value;			
		}

		public double Parameter { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private Func<double, double> _Function;
		public Func<double, double> Function
		{
			get => _Function;
			set
			{
				if (value != _Function)
				{
					_Function = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Function)));
				}
			}
		}

		public double Value => Function(Parameter);
	}

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

	public interface IBuild
	{
		int BuildTime { get; set; }
		Cost BuildCost { get; set; }
	}
}
