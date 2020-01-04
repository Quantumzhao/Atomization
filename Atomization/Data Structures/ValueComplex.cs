using System.ComponentModel;

namespace Atomization
{
	public class ValueComplex
	{
		public ValueComplex(double initialValue = 0)
		{
			value_Object = new VM<double>(initialValue);
			Maximum = double.MaxValue;
			Minimum = double.MinValue;
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
		public double Maximum { get; set; }
		public double Minimum { get; set; }
		public Growth Growth { get; set; }

		//public bool IsSame(IValue viewModel)
		//{
		//	throw new NotImplementedException();
		//}
	}
}