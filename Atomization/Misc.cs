using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;

namespace Atomization
{
	public delegate void ConstructionCompleted(ConstructableObject deployableObject);

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

	public class ValueComplex : IViewModel
	{
		public ValueComplex(double initialValue = 0)
		{
			value_Object = new VM<double>(initialValue);
			Maximum = new VM<double>(double.MaxValue);
			Minimum = new VM<double>(double.MinValue);
			Growth = new Growth();
		}

		private VM<double> value_Object;

		public event PropertyChangedEventHandler PropertyChanged;

		public VM<double> Value_Object
		{
			get => value_Object;
			set
			{
				if (value != value_Object)
				{
					value_Object = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value_Object)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value_Numeric)));
				}
			}
		}
		public double Value_Numeric
		{
			get => Value_Object.ObjectData;
			set
			{
				if (value != Value_Object.ObjectData)
				{
					this.Value_Object.ObjectData = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value_Object)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value_Numeric)));
				}
			}
		}
		public VM<double> Maximum { get; set; }
		public VM<double> Minimum { get; set; }
		public Growth Growth { get; set; }

		public bool IsSame(IViewModel viewModel)
		{
			throw new NotImplementedException();
		}
	}

	public class Growth : IViewModel
	{
		public VMDictionary<VM<string>, VM<double>> Items { get; set; }
			= new VMDictionary<VM<string>, VM<double>>();

		public VM<double> this[string name]
		{
			get => Items.Find(p => p.Key.ObjectData == name).Value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			Items.OnCollectionChanged(e);
		}

		public bool Contains(string name)
		{
			return Items.Where(p => p.Key.ObjectData == name).Count() != 0;
		}
		public bool Contains(VM<double> value)
		{
			return Items.Where(p => p.Value.ObjectData.Equals(value)).Count() != 0;
		}

		public void AddValue(string name, double number)
		{
			VM<string> vMName = new VM<string>(name);
			if (Contains(name))
			{
				Items[vMName].ObjectData += number;
			}
			else
			{
				var pair = new VMKVPair<VM<string>, VM<double>>(vMName, new VM<double>(number));
				pair.PropertyChanged += (sender, e) => 
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
				Items.Add(pair);
			}
		}

		public bool IsSame(IViewModel viewModel)
		{
			throw new NotImplementedException();
		}

		public int Sum
		{
			get
			{
				double sum = 0;
				foreach (var item in Items)
				{
					sum += item.Value.ObjectData;
				}
				return (int)sum;
			}
		}
	}
	public class Cost : IViewModel
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

		public bool IsSame(IViewModel viewModel)
		{
			throw new NotImplementedException();
		}
	}

	public class Expression : INotifyPropertyChanged
	{
		public Expression(VM<double> para, Func<VM<double>, double> function)
		{
			Parameter = para;
			this.function = function;

			para.PropertyChanged += (sender, e) => PropertyChanged?.Invoke(
				this, new PropertyChangedEventArgs(nameof(Parameter)));
		}
		public Expression(double value)
		{
			function = p => value;			
		}

		public VM<double> Parameter { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private Func<VM<double>, double> function;
		public Func<VM<double>, double> Function
		{
			get => function;
			set
			{
				if (value != function)
				{
					function = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Function)));
				}
			}
		}

		public double Value => Function(Parameter);
	}

	public class VM<V> : IViewModel
	{
		public VM(V value)
		{
			if (value is INotifyPropertyChanged)
			{
				(value as INotifyPropertyChanged).PropertyChanged += (sender, e) => 
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ObjectData)));
			}
			objectData = value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private V objectData;
		public V ObjectData
		{
			get => objectData;
			set
			{
				if (!value.Equals(objectData))
				{
					objectData = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(objectData)));
				}
			}
		}

		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}

		public bool IsSame(IViewModel viewModel)
		{
			if (viewModel is VM<V>)
			{
				VM<V> vM = viewModel as VM<V>;
				return vM.objectData.Equals(objectData);
			}

			return false;
		}

		public override string ToString()
		{
			return objectData.ToString();
		}
	}

	public class VMList<V> : List<V>, INotifyCollectionChanged, IViewModel
		where V : IViewModel
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public new void Add(V newValue)
		{
			base.Add(newValue);
			CollectionChanged?.Invoke(this, 
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Add, newValue
				)
			);
		}

		public new void Remove(V oldValue)
		{
			var vmOldValue = Find(v => v.IsSame(new VM<V>(oldValue)));
			CollectionChanged?.Invoke(this,
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Remove, oldValue
				)
			);
		}

		public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}

		public bool IsSame(IViewModel viewModel)
		{
			throw new NotImplementedException();
		}
	}

	public class VMDictionary<K, V> : VMList<VMKVPair<K, V>>
		where K : IViewModel where V : IViewModel
	{
		public void Add(K key, V value)
		{
			var pair = new VMKVPair<K, V>(key, value);
			Add(pair);
		}

		public void Remove(K key)
		{
			var pair = Find(p => p.Key.Equals(key));
			Remove(pair);
		}

		public V this[K key]
		{
			get => Find(pair => pair.Key.IsSame(key)).Value;
			set
			{
				if (!value.IsSame(this[key]))
				{
					this[key] = value;
				}
			}
		}

		public bool ContainsKey(K key)
		{
			foreach (var item in this)
			{
				if (item.Key.IsSame(key))
				{
					return true;
				}
			}

			return false;
		}

		public new bool Contains(VMKVPair<K, V> keyValuePair)
		{
			foreach (var item in this)
			{
				if (item.IsSame(keyValuePair))
				{
					return true;
				}
			}

			return false;
		}
	}

	public class VMKVPair<K, V> : IViewModel where K : IViewModel where V : IViewModel
	{
		public VMKVPair(K key, V value)
		{
			key.PropertyChanged += (sender, e) =>
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Key)));
			this.key = key;
			value.PropertyChanged += (sender, e) =>
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			this.value = value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private K key;
		public K Key
		{
			get => key;
			set
			{
				key = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Key)));
			}
		}

		private V value;
		public V Value
		{
			get => value;
			set
			{
				this.value = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}

		public bool IsSame(IViewModel viewModel)
		{
			if (viewModel is VMKVPair<K, V>)
			{
				var vm = viewModel as VMKVPair<K, V>;
				return vm.key.IsSame(key) && vm.value.IsSame(value);
			}

			return false;
		}
	}

	public interface IViewModel : INotifyPropertyChanged
	{
		bool IsSame(IViewModel viewModel);
	}

	public interface IBuild : IViewModel
	{
		int BuildTime { get; set; }
		Cost BuildCost { get; set; }
	}

	public abstract class ConstructableObject : IBuild
	{
		public event ConstructionCompleted ConstructionCompleted;

		protected VM<int> buildTime;
		public int BuildTime
		{
			get => buildTime.ObjectData;
			set
			{
				if (value == 0)
				{
					ConstructionCompleted?.Invoke(this);
					buildTime.ObjectData = -1;
					return;
				}
				else if (buildTime.ObjectData == -1)
				{
					return;
				}

				if (value != buildTime.ObjectData)
				{
					buildTime.ObjectData = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuildTime)));
				}
			}
		}

		protected Cost buildCost;
		public Cost BuildCost
		{
			get => buildCost;
			set
			{
				if (value != buildCost)
				{
					buildCost = value;
					buildCost.PropertyChanged += (sender, e) => 
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuildCost)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuildCost)));
				}
			}
		}

		protected Cost maintenance;
		public Cost Maintenance
		{
			get => maintenance;
			set
			{
				if (value != maintenance)
				{
					maintenance = value;
					maintenance.PropertyChanged += (sender, e) =>
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(maintenance)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(maintenance)));
				}
			}
		}

		public bool IsMine { get; set; } = true;
		public abstract string TypeName { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public virtual bool IsSame(IViewModel viewModel)
		{
			throw new NotImplementedException();
		}

		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}
	}
}
