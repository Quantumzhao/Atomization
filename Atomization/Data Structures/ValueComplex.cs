using System.ComponentModel;

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
		public Growth Growth { get; private set; }
	}
}