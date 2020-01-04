using System.ComponentModel;

namespace Atomization
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

		public double Maximum { get; set; }
		public double Minimum { get; set; }
		public Growth Growth { get; set; }
	}
}