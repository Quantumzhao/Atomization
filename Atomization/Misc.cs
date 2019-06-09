using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System;

namespace Atomization
{
	public delegate void ValueChange<in S, T>(S sender, T previousValue, T newValue);
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

			public event ValueChange<object, double> OnValueChanged
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
				public event ValueChange<object, double> OnValueChanged;

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
			Value_Object = new InternalValue(initialValue);
			Maximum = new InternalValue(double.MaxValue);
			Minimum = new InternalValue(double.MinValue);
			Growth = new Value_Growth(Value_Object);
		}

		public InternalValue Value_Object { get; set; }
		public double Value_Numerical
		{
			get => Value_Object.Value;
			set => this.Value_Object.Value = value;
		}
		public InternalValue Maximum { get; set; }
		public InternalValue Minimum { get; set; }
		public Value_Growth Growth { get; set; }

		public event ValueChange<object, double> OnValueChanged
		{
			add => this.Value_Object.ValueChange += value;
			remove => this.Value_Object.ValueChange -= value;
		}
	}

	public class InternalValue
	{
		public InternalValue(double initialValue)
		{
			value = initialValue;
		}

		public event ValueChange<object, double> ValueChange;

		private double value;
		public double Value
		{
			get => value;
			set
			{
				if (value != this.value)
				{
					ValueChange?.Invoke(this, this.value, value);
					this.value = value;
				}
			}
		}

		public void OnValueChanged(object sender, double oldValue, double newValue)
		{
			ValueChange?.Invoke(sender, oldValue, newValue);
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

		public void AddValue(string name, double number)
		{
			var value = new InternalValue(number);
			value.ValueChange += (n, pv, nv) => CollectionChanged?.Invoke(
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
	public class Cost : INotifyCollectionChanged
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
			Values = new ObservableCollection<Expression>();
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
					Values[i].OnValueChanged += (sender, oldValue, newValue)
						=> CollectionChanged?.Invoke(
							sender,
							new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, oldValue, newValue)
						);
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

		// stores value, and the information of whether it is in absolute value
		public readonly ObservableCollection<Expression> Values;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

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

	public class Expression
	{
		public Expression(InternalValue para, Func<InternalValue, double> function)
		{
			Parameter = para;
			this.function = function;

			para.ValueChange += OnValueChanged;
		}
		public Expression(double value)
		{
			function = p => value;			
		}

		public InternalValue Parameter { get; set; }

		private Func<InternalValue, double> function;
		public Func<InternalValue, double> Function
		{
			get => function;
			set
			{
				double tempV = Value;
				function = value;
				OnValueChanged?.Invoke(this, tempV, Value);
			}
		}

		public double Value => Function(Parameter);

		public event ValueChange<object, double> OnValueChanged;
	}
}
