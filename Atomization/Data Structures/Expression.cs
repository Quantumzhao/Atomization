using System;
using System.ComponentModel;

namespace Atomization.DataStructures
{
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

		public void Add(Expression expression)
		{
			Function = value => Function(value) + expression.Function(value);
		}
	}
}