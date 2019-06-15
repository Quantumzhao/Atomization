using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
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

		public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
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
					var temp = this.value;
					this.value = value;
					ValueChange?.Invoke(this, temp, value);
				}
			}
		}

		public void OnValueChanged(object sender, double oldValue, double newValue)
		{
			ValueChange?.Invoke(sender, oldValue, newValue);
		}
	}

	public class Value_Growth
	{
		public Value_Growth(InternalValue bindedValue)
		{
			this.bindedValue = bindedValue;
		}

		private InternalValue bindedValue;

		public GameObjectDictionary Values { get; set; }
			= new GameObjectDictionary();

		public InternalValue this[string name]
		{
			get => Values[name];
		}

		public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			Values.OnCollectionChanged(e);
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
			if (Values.ContainsKey(name))
			{
				Values[name].Value += number;
			}
			else
			{
				Values.Add(name, new InternalValue(number));
			}
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
					Values[i].OnValueChanged += (sender, oldValue, newValue) => 
						Values.OnCollectionChanged(
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

		public readonly GameObjectList<Expression> Values;

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

	public class GameObject<T> : INotifyPropertyChanged
	{
		public GameObject(T wrappedObject)
		{
			value = wrappedObject;
		}

		private T value;
		public T Value
		{
			get => value;
			set
			{
				if (!this.value.Equals(value))
				{
					this.value = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class GameObjectDictionary : Dictionary<string, InternalValue>, INotifyCollectionChanged
	{
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public new void Add(string key, InternalValue value)
		{
			//value.ValueChange += (sender, oldV, newV) => CollectionChanged?.Invoke(this, 
			//	new NotifyCollectionChangedEventArgs(
			//		NotifyCollectionChangedAction.Replace, sender, sender
			//	)
			//);
			base.Add(key, value);
			CollectionChanged?.Invoke(this, 
				new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
		}

		public new void Remove(string key)
		{
			base.Remove(key);
			CollectionChanged?.Invoke(this, 
				new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
		}

		public new InternalValue this[string key]
		{
			get => base[key];
			set
			{
				if (value.Equals(base[key]))
				{
					base[key] = value;
					CollectionChanged?.Invoke(this, 
						new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));
				}
			}
		}

		public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}
	}

	public class DataUpdater
	{
		private Dictionary<string, DataBinder> UiElements = new Dictionary<string, DataBinder>();

		public object GetUiElement(string name)
		{
			return UiElements[name].UiElement;
		}

		/// <summary>
		///		gets and sets the source of the field of UIElement that needs to be updated
		/// </summary>
		/// <param name="bindedUiElementName">
		///		the name of the UIElement
		/// </param>
		/// <returns>
		///		the source of the binded field
		/// </returns>
		public object this[string bindedUiElementName]
		{
			get => UiElements[bindedUiElementName].Source;
			set
			{
				if (value != UiElements[bindedUiElementName].Source)
				{
					UiElements[bindedUiElementName].UiElement = value;
					UiElements[bindedUiElementName].Source = value;
				}
			}
		}

		public void Add(object uiElement, string elementName, object source)
		{
			UiElements.Add(elementName, new DataBinder(ref uiElement, source));
		}

		public void Refresh(string bindedUiElementName)
		{
			UiElements[bindedUiElementName].UiElement = UiElements[bindedUiElementName].Source;
		}

		public class DataBinder
		{
			public DataBinder(ref object uiElement, object source)
			{
				UiElement = uiElement;
				uiElement = source;
				Source = source;
			}

			public object UiElement { get; set; }
			public object Source { get; set; }
		}
	}
}
