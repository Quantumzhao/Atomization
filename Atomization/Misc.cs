using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public delegate void OnValueChanged<in S, T>(S sender, T previousValue, T newValue);
	public delegate void OnValueAdded<T>(IEnumerable<T> sender, T addedItem);
	public delegate void OnValueDeleted<T>(IEnumerable<T> sender, T deletedItem);
	public class GameObjectList<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		public new int Capacity { get; set; } = 0;
		public bool IsLimitedCapacity { get; set; } = false;

		//public event Action<GameObjectList<T>, T, T> OnItemChanged;
		//public event Action<GameObjectList<T>, T> OnItemAdded;
		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;
		//public event Action<GameObjectList<T>, T> OnItemRemoved;

		public new bool Add(T item)
		{
			if (Count >= Capacity && IsLimitedCapacity)
			{
				return false;
			}
			else
			{
				base.Add(item);
				//OnItemAdded?.Invoke(this, item);
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
			//OnItemRemoved?.Invoke(this, item);
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
	}

	public class InternalValue
	{
		public InternalValue(object parent, double initialValue)
		{
			this.parent = parent;
			value = initialValue;
		}

		private object parent;
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

		public InternalValue this[string name]
		{
			get => Values[name];
		}

		public bool Contains(string name)
		{
			return Values.ContainsKey(name);
		}
		public bool Contains(InternalValue value)
		{
			return Values.ContainsValue(value);
		}

		public void Add(string name, float number)
		{
			var value = new InternalValue(parent, number);
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
	public class Cost
	{
		public Cost(
			object parent,
			string name,
			double economy = 0,
			double hiEduPopu = 0,
			double army = 0,
			double navy = 0,
			double rawMaterial = 0,
			double nuclearMaterial = 0,
			double stability = 0
		)
		{
			if (economy != 0) Economy = new InternalValue(parent, economy);
			if (hiEduPopu != 0) HiEduPopu = new InternalValue(parent, hiEduPopu);
			if (army != 0) Army = new InternalValue(parent, army);
			if (navy != 0) Navy = new InternalValue(parent, navy);
			if (rawMaterial != 0) RawMaterial = new InternalValue(parent, rawMaterial);
			if (nuclearMaterial != 0) NuclearMaterial = new InternalValue(parent, nuclearMaterial);
			if (stability != 0) Stability = new InternalValue(parent, stability);
		}

		public InternalValue Economy { get; set; }
		public InternalValue HiEduPopu { get; set; }
		public InternalValue Army { get; set; }
		public InternalValue Navy { get; set; }
		public InternalValue Food { get; set; }
		public InternalValue RawMaterial { get; set; }
		public InternalValue NuclearMaterial { get; set; }
		public InternalValue Stability { get; set; }
	}
}
