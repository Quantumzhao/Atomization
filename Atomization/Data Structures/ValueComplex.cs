using System;
using System.ComponentModel;
using System.Linq;

namespace Atomization.DataStructures
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
			Array.ForEach(_Values, vc => vc = new ValueComplex());
		}

		public const int NUM_VALUES = 11;

		private readonly ValueComplex[] _Values = new ValueComplex[NUM_VALUES];

		public ValueComplex Economy
		{
			get => _Values[0];
			protected set => _Values[0] = value;
		}
		public ValueComplex Population
		{
			get => _Values[1];
			protected set => _Values[1] = value;
		}
		public ValueComplex Army
		{
			get => _Values[2];
			protected set => _Values[2] = value;
		}
		public ValueComplex Navy
		{
			get => _Values[3];
			protected set => _Values[3] = value;
		}

		public ValueComplex Food
		{
			get => _Values[4];
			protected set => _Values[4] = value;
		}

		public ValueComplex RawMaterial
		{
			get => _Values[5];
			protected set => _Values[5] = value;
		}

		public ValueComplex NuclearMaterial
		{
			get => _Values[6];
			protected set => _Values[6] = value;
		}

		public ValueComplex Stability
		{
			get => _Values[7];
			protected set => _Values[7] = value;
		}

		public ValueComplex Nationalism
		{
			get => _Values[8];
			protected set => _Values[8] = value;
		}

		public ValueComplex Satisfaction
		{
			get => _Values[9];
			protected set => _Values[9] = value;
		}

		public ValueComplex Bureaucracy
		{
			get => _Values[10];
			protected set => _Values[10] = value;
		}

		public ValueComplex this[int index] => _Values[index];
	}
}