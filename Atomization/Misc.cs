using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Atomization
{
	public delegate void OnValueChanged<in S, T>(S sender, T previousValue, T newValue);
	public class GameObjectList<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		public new int Capacity { get; set; } = 0;
		public bool IsLimitedCapacity { get; set; } = false;

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public new bool Add(T item)
		{
			if (Count >= Capacity && IsLimitedCapacity)
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
			CollectionChanged?.Invoke(
				this,
				new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item)
			);
			return base.Remove(item);
		}

		public class ValueComplex
		{
			public ValueComplex(Nation parent, double initialValue = 0)
			{
				value = new InternalValue(parent, initialValue);
				Maximum = new InternalValue(parent, double.MaxValue);
				Minimum = new InternalValue(parent, double.MinValue);
				Growth = new Value_Growth(parent, value);
			}

			private InternalValue value;
			public double Value
			{
				get => value.Value;
				set => this.value.Value = value;
			}
			public InternalValue Maximum { get; set; }
			public InternalValue Minimum { get; set; }
			public Value_Growth Growth { get; set; }

			public event OnValueChanged<object, double> OnValueChanged
			{
				add => this.value.OnValueChanged += value;
				remove => this.value.OnValueChanged -= value;
			}

			public class InternalValue
			{
				public InternalValue(Nation parent, double initialValue)
				{
					this.parent = parent;
					value = initialValue;
				}

				private Nation parent;
				public event OnValueChanged<object, double> OnValueChanged;

				private double value;
				public double Value
				{
					get => value;
					set
					{
						if (value != this.value)
						{
							OnValueChanged?.Invoke(parent, this.value, value);
							this.value = value;
						}
					}
				}
			}

			public class Value_Growth : INotifyCollectionChanged
			{
				public Value_Growth(Nation parent, InternalValue bindedValue)
				{
					this.parent = parent;
					this.bindedValue = bindedValue;
				}

				private InternalValue bindedValue;
				private Nation parent;

				public event NotifyCollectionChangedEventHandler CollectionChanged;

				public Dictionary<string, InternalValue> Values { get; set; }
					= new Dictionary<string, InternalValue>();

				public void Add(string name, int number)
				{
					var value = new InternalValue(parent, number);
					value.OnValueChanged += (n, pv, nv) => CollectionChanged?.Invoke(
						this,
						new NotifyCollectionChangedEventArgs(
							NotifyCollectionChangedAction.Replace,
							value
						)
					);
					Values.Add(name, value);
					CollectionChanged?.Invoke(
						this,
						new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value)
					);
				}
				public void Add(string name, double percent)
				{
					Add(name, (int)(bindedValue.Value * percent));
				}

				public int Sum
				{
					get
					{
						double sum = 0;
						foreach (var item in Values)
						{
							sum += item.Value.Value;
						}
						return (int)sum;
					}
				}
			}
		}
	}

	public class ValueComplex
	{
		public ValueComplex(double initialValue = 0)
		{
			value = new InternalValue(initialValue);
			Maximum = new InternalValue(double.MaxValue);
			Minimum = new InternalValue(double.MinValue);
			Growth = new Value_Growth(value);
		}

		private InternalValue value;
		public double Value
		{
			get => value.Value;
			set => this.value.Value = value;
		}
		public InternalValue Maximum { get; set; }
		public InternalValue Minimum { get; set; }
		public Value_Growth Growth { get; set; }

		public event OnValueChanged<object, double> OnValueChanged
		{
			add => this.value.OnValueChanged += value;
			remove => this.value.OnValueChanged -= value;
		}
	}

	public class InternalValue
	{
		public InternalValue(double initialValue)
		{
			value = initialValue;
		}

		public event OnValueChanged<object, double> OnValueChanged;

		private double value;
		public double Value
		{
			get => value;
			set
			{
				if (value != this.value)
				{
					OnValueChanged?.Invoke(this, this.value, value);
					this.value = value;
				}
			}
		}
	}

	public class Value_Growth : INotifyCollectionChanged
	{
		public Value_Growth(InternalValue bindedValue)
		{
			this.bindedValue = bindedValue;
		}

		private InternalValue bindedValue;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public Dictionary<string, InternalValue> Values { get; set; }
			= new Dictionary<string, InternalValue>();

		public InternalValue this[string name]
		{
			get => Values[name];
		}

		public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(sender, e);
		}

		public bool Contains(string name)
		{
			return Values.ContainsKey(name);
		}
		public bool Contains(InternalValue value)
		{
			return Values.ContainsValue(value);
		}

		public void Add_AbsValue(string name, double number)
		{
			var value = new InternalValue(number);
			value.OnValueChanged += (n, pv, nv) => CollectionChanged?.Invoke(
				this,
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Add,
					value
				)
			);
			if (Values.ContainsKey(name))
			{
				Values[name].Value += number;
			}
			else
			{
				Values.Add(name, value);
			}
			CollectionChanged?.Invoke(
				this,
				new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value)
			);
		}
		public void Add_Percent(string name, double percent)
		{
			Add_AbsValue(name, (int)(bindedValue.Value * percent));
		}

		public int Sum
		{
			get
			{
				double sum = 0;
				foreach (var item in Values)
				{
					sum += item.Value.Value;
				}
				return (int)sum;
			}
		}
	}
	public class Cost
	{
		public Cost(
			string name,
			double economy = 0,
			bool isEconomyAbs = true,
			double hiEduPopu = 0,
			bool isHiEduPopuAbs = true,
			double army = 0,
			bool isArmyAbs = true,
			double navy = 0,
			bool isNavyAbs = true,
			double food = 0,
			bool isFoodAbs = true,
			double rawMaterial = 0,
			bool isRawMaterialAbs = true,
			double nuclearMaterial = 0,
			bool isNuclearMaterialAbs = true,
			double stability = 0,
			bool isStabilityAbs = true
		)
		{
			Name = name;

			// the number of properties
			Values = new ObservableCollection<KeyValuePair<InternalValue, bool>>();
			for (int i = 0; i < 8; i++)
			{
				Values.Add(new KeyValuePair<InternalValue, bool>(null, true));
			}

			Values[0] = new KeyValuePair<InternalValue, bool>(new InternalValue(economy), isEconomyAbs);
			Values[1] = new KeyValuePair<InternalValue, bool>(new InternalValue(hiEduPopu), isHiEduPopuAbs);
			Values[2] = new KeyValuePair<InternalValue, bool>(new InternalValue(army), isArmyAbs);
			Values[3] = new KeyValuePair<InternalValue, bool>(new InternalValue(navy), isNavyAbs);
			Values[4] = new KeyValuePair<InternalValue, bool>(new InternalValue(food), isFoodAbs);
			Values[5] = new KeyValuePair<InternalValue, bool>(new InternalValue(rawMaterial), isRawMaterialAbs);
			Values[6] = new KeyValuePair<InternalValue, bool>(new InternalValue(nuclearMaterial), isNuclearMaterialAbs);
			Values[7] = new KeyValuePair<InternalValue, bool>(new InternalValue(stability), isStabilityAbs);

		}

		// stores value, and the information of whether it is in absolute value
		public readonly ObservableCollection<KeyValuePair<InternalValue, bool>> Values;

		public string Name { get; set; }
		public InternalValue Economy
		{
			get => Values[0].Key;
		}
		public InternalValue HiEduPopu
		{
			get => Values[1].Key;
		}

		public InternalValue Army
		{
			get => Values[2].Key;
		}

		public InternalValue Navy
		{
			get => Values[3].Key;
		}

		public InternalValue Food
		{
			get => Values[4].Key;
		}

		public InternalValue RawMaterial
		{
			get => Values[5].Key;
		}

		public InternalValue NuclearMaterial
		{
			get => Values[6].Key;
		}

		public InternalValue Stability
		{
			get => Values[7].Key;
		}
	}

	public class Expression
	{
		public Expression(InternalValue para, double coefficient)
		{
			Parameter = para;
			Coefficient = coefficient;
		}

		public InternalValue Parameter { get; set; }
		public double Coefficient { get; set; }

		public 
	}
}
