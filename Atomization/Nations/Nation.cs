using Atomization.DataStructures;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;

namespace Atomization
{
	public abstract class Region
	{
		public string Name { get; set; }
	}

	/// <summary>
	///		Defining the behaviors of all waters. 
	/// </summary>
	public class Waters : Region
	{
		// The number of international waters in this game is set to be 5
		public const int NUM_INTL_WATERS = 5;
		// Each sea belongs to a certain nation. 
		// if the property is null, then it is international water
		public Nation Affiliation { get; set; }
	}

	/// <summary>
	///		The base class of all nations, including regular nation and superpower
	/// </summary>
	public abstract class Nation : Region
	{
		public const int NUM_VALUES = 11;
		public Nation()
		{
			// Initialize its territorial water, and name the water as well
			TerritorialWaters = new Waters() { Name = Data.WatersNames.Dequeue() };
			TerritorialWaters.Affiliation = this;
			Data.Regions.Add(TerritorialWaters);
		}

		// null stands for independence
		public Superpower Affiliation { get; set; } = null;

		public Waters TerritorialWaters { get; }

		public readonly ValueComplexNTuple NationalIndices = new ValueComplexNTuple();
		public readonly ValueComplexNTuple OutdatedNationalIndices = new ValueComplexNTuple();

		//public ConstrainedList<Effect> ExpenditureAndRevenue = new ConstrainedList<Effect>();

		public double Tactics { get; set; }

		public double AdjustedBureaucracyIndex => 0.1 * Math.Pow(1.2, NationalIndices.Bureaucracy.CurrentValue);

		public TaskSequence TaskSequence { get; private set; } = new TaskSequence();
		public TechTreeNode TechTree { get; }
		public Dictionary<string, object> Reserve { get; } = new Dictionary<string, object>();

		public void AddCostOfExecution(Effect cost)
		{
			for (int i = 0; i < cost.Values.Length; i++)
			{
				NationalIndices[i].CurrentValue += cost.Values[i].Value;
			}
		}

		public void UpdateValue(ValueComplex nationalIndex)
		{
			Data.Me.TaskSequence.Add(Task.Create(Census.Create(nationalIndex), "Generating statistics"));
		}
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
			
		}
	}
}
