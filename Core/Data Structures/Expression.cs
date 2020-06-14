using System;
using System.ComponentModel;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LCGuidebook.Core.DataStructures
{
	public class Expression : INotifyPropertyChanged
	{
		private const string CONST_COEFFICIENT = "It is a constant function";

		public Expression(double coefficient, Func<double, double> function, bool isReadOnly = false)
		{
			Coefficient = coefficient;
			this._Function = function;
			IsReadOnly = isReadOnly;
		}
		public Expression(double value)
		{
			_Function = p => value;
			IsReadOnly = false;
		}
		public readonly bool IsReadOnly;

		private double _Coefficient;
		public double Coefficient
		{
			get
			{
				if (!IsReadOnly)
				{
					return _Coefficient;
				}
				else
				{
					throw new InvalidOperationException(CONST_COEFFICIENT);
				}
			}

			set
			{
				if (!IsReadOnly)
				{
					_Coefficient = value;
				}
				else
				{
					throw new InvalidOperationException(CONST_COEFFICIENT);
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private Func<double, double> _Function;

		public Func<double, double> Function
		{
			get => _Function;
			set
			{
				if (IsReadOnly)
				{
					throw new InvalidOperationException(CONST_COEFFICIENT);
				}
				else if (value != _Function)
				{
					_Function = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Function)));
				}
			}
		}

		public double Value => _Function(_Coefficient);

		public static explicit operator Expression(double value) => new Expression(value);

		public void Add(Expression expression)
		{
			Function = value => Function(value) + expression.Function(value);
		}

		public static Expression operator +(Expression exp1, Expression exp2)
		{
			return new Expression(0, v => exp1.Function(exp1._Coefficient) + exp2.Function(exp2._Coefficient), true);
		}
	}
}