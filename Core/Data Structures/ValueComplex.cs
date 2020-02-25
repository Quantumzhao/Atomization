using System;
using System.ComponentModel;
using System.Linq;

namespace LCGuidebook.Core.DataStructures
{
	public class ValueComplex
	{
		public ValueComplex(double initialValue = 0)
		{
			_CurrentValue = initialValue;
			Maximum = double.MaxValue;
			Minimum = double.MinValue;
			Growth = new Growth();
		}

		private double _CurrentValue;

		public event PropertyChangedEventHandler PropertyChanged;

		public double CurrentValue
		{
			get => _CurrentValue;
			set
			{
				if (value != _CurrentValue)
				{
					_CurrentValue = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentValue)));
				}
			}
		}

		private double _Maximum;
		public double Maximum
		{
			get => _Maximum;
			set
			{
				if (_Maximum != value)
				{
					_Maximum = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Maximum)));
				}
			}
		}

		private double _Minimum;
		public double Minimum
		{
			get => _Minimum;
			set
			{
				if (_Minimum != value)
				{
					_Minimum = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Minimum)));
				}
			}
		}

		public Growth Growth { get; }

		public void Update(ValueComplex actualValue)
		{
			CurrentValue = actualValue.CurrentValue;
			Maximum = actualValue.Maximum;
			Minimum = actualValue.Minimum;
			foreach (var pair in actualValue.Growth.Items)
			{
				Growth.AddTerm(pair.Key, pair.Value);
				var dic = Growth.Items;
				if (dic.ContainsKey(pair.Key))
				{
					dic[pair.Key] = pair.Value;
				}
				else
				{
					dic.Add(pair.Key, pair.Value);
				}
			}
		}
	}

	public class ValueComplexNTuple
	{
		public ValueComplexNTuple()
		{
			for (int i = 0; i < NUM_VALUES; i++)
			{
				_Values[i] = new ValueComplex();
			}
		}

		public const int NUM_VALUES = 11;

		private readonly ValueComplex[] _Values = new ValueComplex[NUM_VALUES];

		public ValueComplex this[MainIndexType type] => _Values[(int)type];
		public ValueComplex this[int type] => _Values[type];
	}

	public enum MainIndexType
	{
		Economy = 0,
		Population = 1,
		Army = 2,
		Navy = 3,
		Food = 4,
		RawMaterial = 5,
		NuclearMaterial = 6,
		Stability = 7,
		Nationalism = 8,
		Satisfaction = 9,
		Bureaucracy = 10
	}
}