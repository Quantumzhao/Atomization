using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LCGuidebook.Core.DataStructures
{
	public class CostOfStage
	{
		public CostOfStage(Effect longTermEffect, Effect shortTermEffect, Expression requiredTime)
		{
			LongTermEffect = longTermEffect;
			ShortTermEffect = shortTermEffect;
			RequiredTime = requiredTime;
		}

		public Effect LongTermEffect { get; }
		public Effect ShortTermEffect { get; }
		public Expression RequiredTime { get; }

		public static CostOfStage operator +(CostOfStage cost1, CostOfStage cost2)
		{
			return new CostOfStage(
				cost1.LongTermEffect + cost2.LongTermEffect,
				cost1.ShortTermEffect + cost2.ShortTermEffect,
				cost1.RequiredTime + cost2.RequiredTime
			);
		}
	}

	public enum TypesOfCostOfStage
	{
		Manufacture,
		Transportation,
		Deployment,
		Maintenance
	}
}