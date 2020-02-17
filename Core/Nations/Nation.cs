using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System;

namespace LCGuidebook.Core
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
			TerritorialWaters = new Waters() { Name = ResourceManager.WatersNames.Dequeue() };
			TerritorialWaters.Affiliation = this;
			ResourceManager.Regions.Add(TerritorialWaters);
		}

		protected readonly Dictionary<string, IUniqueObject> _Reserve = new Dictionary<string, IUniqueObject>();

		public readonly ValueComplexNTuple NationalIndices = new ValueComplexNTuple();
		public readonly ValueComplexNTuple OutdatedNationalIndices = new ValueComplexNTuple();
		// null stands for independence
		public Superpower Affiliation { get; set; } = null;

		public Waters TerritorialWaters { get; }

		//public ConstrainedList<Effect> ExpenditureAndRevenue = new ConstrainedList<Effect>();

		public double Tactics { get; set; }

		public double AdjustedBureaucracyIndex => 0.1 * Math.Pow(1.2, NationalIndices.Bureaucracy.CurrentValue);

		public TaskSequence TaskSequence { get; } = new TaskSequence();

		// considering replace this data structure with command group later
		[Obsolete]
		public TechTreeNode TechTree { get; }

		public CommandGroup Operations { get; }
		public CommandGroup Intelligence { get; }
		public CommandGroup Defence { get; }
		public CommandGroup Technology { get; }
		public CommandGroup Domestic { get; }
		public CommandGroup Diplomacy { get; }

		//public void AddCostOfExecution(Effect cost)
		//{
		//	for (int i = 0; i < cost.Values.Length; i++)
		//	{
		//		NationalIndices[i].CurrentValue += cost.Values[i].Value;
		//	}
		//}

		public void UpdateValue(int nationalIndex)
		{
			Census census = new Census("Generating statistics", nationalIndex);
			ResourceManager.Me.TaskSequence.AddNewTask(census);
		}
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
			
		}
	}
}
