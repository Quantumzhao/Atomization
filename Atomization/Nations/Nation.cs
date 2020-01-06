using Atomization.DataStructures;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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

		#region Value Definitions
		public ValueComplex[] Values = new ValueComplex[NUM_VALUES];
		public ConstrainedList<Effect> ExpenditureAndRevenue = new ConstrainedList<Effect>();
		public ValueComplex Economy
		{
			get => Values[0];
			protected set => Values[0] = value;
		}
		public ValueComplex HiEduPopu
		{
			get => Values[1];
			protected set => Values[1] = value;
		}
		public ValueComplex Army
		{
			get => Values[2];
			protected set => Values[2] = value;
		}
		public ValueComplex Navy
		{
			get => Values[3];
			protected set => Values[3] = value;
		}
		public double Tactics { get; set; }
		public ValueComplex Food
		{
			get => Values[4];
			protected set => Values[4] = value;
		}

		public ValueComplex RawMaterial
		{
			get => Values[5];
			protected set => Values[5] = value;
		}

		public ValueComplex NuclearMaterial
		{
			get => Values[6];
			protected set => Values[6] = value;
		}

		public ValueComplex Stability
		{
			get => Values[7];
			protected set => Values[7] = value;
		}

		#region Public Opinions
		public ValueComplex Nationalism
		{
			get => Values[8];
			protected set => Values[8] = value;
		}

		public ValueComplex Satisfaction
		{
			get => Values[9];
			protected set => Values[9] = value;
		}
		#endregion

		public ValueComplex Bureaucracy
		{
			get => Values[10];
			protected set => Values[10] = value;
		}
		#endregion

		public TaskSequence TaskSequence { get; private set; } = new TaskSequence();

		protected void CreateTask(Stage task)
		{

		}

		public void AddCostOfExecution(Effect cost)
		{
			for (int i = 0; i < cost.Values.Length; i++)
			{
				Values[i].CurrentValue += cost.Values[i].Value;
			}
		}
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
		}
	}
}
