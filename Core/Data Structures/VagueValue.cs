using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;


namespace LCGuidebook.Core.DataStructures
{
	public class VagueValue
	{
		public VagueValue(double initialValue, double meanShift = 0, double stdDev = 1)
		{
			ActualValue = initialValue;
			MeanShift = meanShift;
			StandardDeviation = stdDev;
		}
		public readonly bool IsPositiveTrendFavored;

		private double _MeanShift;
		public double MeanShift
		{
			get => _MeanShift;
			set
			{
				if (_MeanShift != value)
				{
					_MeanShift = value;
					RefreshPreferredValue();
				}
			}
		}

		private double _StandardDeviation;
		public double StandardDeviation
		{
			get => _StandardDeviation;
			set
			{
				if (_StandardDeviation != value)
				{
					_StandardDeviation = value;
					RefreshPreferredValue();
				}
			}
		}

		public double PreferredValue { get; private set; }

		private double _ActualValue;
		public double ActualValue
		{
			get => _ActualValue;
			set
			{
				if (_ActualValue != value)
				{
					_ActualValue = value;
					RefreshPreferredValue();
				}
			}
		}

		private void RefreshPreferredValue()
		{
			double temp;
			do
			{
				temp = Normal.Sample(ActualValue + MeanShift, StandardDeviation);
			} while (IsPositiveTrendFavored ? temp < ActualValue : temp > ActualValue);
			PreferredValue = temp;
		}
	}
}
