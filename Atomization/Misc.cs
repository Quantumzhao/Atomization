using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using Atomization.DataStructures;

namespace Atomization
{
	public delegate void StageFinishedHandler(Stage previous, Stage next, Task task);

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

	/// <summary>
	///		It is only a data structure for <c>Event</c>. 
	///		It should never be used alone
	/// </summary>
	public class Effect
	{
		public Effect(
			Expression economy = null,
			Expression hiEduPopu = null,
			Expression army = null,
			Expression navy = null,
			Expression food = null,
			Expression rawMaterial = null,
			Expression nuclearMaterial = null,
			Expression stability = null,
			Expression nationalism = null,
			Expression satisfaction = null,
			Expression bureaucracy = null
		)
		{
			Values[0] = economy;
			Values[1] = hiEduPopu;
			Values[2] = army;
			Values[3] = navy;
			Values[4] = food;
			Values[5] = rawMaterial;
			Values[6] = nuclearMaterial;
			Values[7] = stability;
			Values[8] = nationalism;
			Values[9] = satisfaction;
			Values[10] = bureaucracy;

			for (int i = 0; i < Nation.NUM_VALUES; i++)
			{
				if (Values[i] == null)
				{
					Values[i] = 0;
				}
			}
		}

		public readonly Expression[] Values = new Expression[Nation.NUM_VALUES];

		public event PropertyChangedEventHandler PropertyChanged;

		public double Economy => Values[0].Value;
		public double HiEduPopu => Values[1].Value;
		public double Army => Values[2].Value;
		public double Navy => Values[3].Value;
		public double Food => Values[4].Value;
		public double RawMaterial => Values[5].Value;
		public double NuclearMaterial => Values[6].Value;
		public double Stability => Values[7].Value;
		public double Nationalism => Values[8].Value;
		public double Satifaction => Values[9].Value;
		public double Bureaucracy => Values[10].Value;
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

		public double Value => _Function(Parameter);

		public static implicit operator Expression(double value) => new Expression(value);
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
	public static class Misc
	{
		public static int Round(double value) => (int)(value + 0.5);
	}

	public interface IExecutable
	{
		void Execute();
	}
}
