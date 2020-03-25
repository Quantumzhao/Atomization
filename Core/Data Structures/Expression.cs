﻿using System;
using System.ComponentModel;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

namespace LCGuidebook.Core.DataStructures
{
	public class Expression : INotifyPropertyChanged
	{
		public Expression(double constant, Func<double, double> function)
		{
			Constant = constant;
			this._Function = function;
		}
		public Expression(double value)
		{
			_Function = p => value;
		}

		public double Constant { get; set; }

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

		public double Value => _Function(Constant);

		public static explicit operator Expression(double value) => new Expression(value);

		public void Add(Expression expression)
		{
			Function = value => Function(value) + expression.Function(value);
		}
	}
}