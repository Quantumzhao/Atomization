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

			// set the data binding of gov exp and rev
			// when the national government is entitled a new expenditure/revenue, 
			//     it adds the new item to this list, and update the binded property
			ExpenditureAndRevenue.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					AddExpenditureAndRevenue(e.NewItems[0] as Impact);
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					RemoveExpenditureAndRevenue(e.OldItems[0] as Impact);
				}
			};

			ConstructionSequence.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					(e.NewItems[0] as ConstructableObject).ConstructionCompleted += item =>
					(sender as ObservableCollection<ConstructableObject>).Remove(item);
				}
			};
		}

		// null stands for independence
		public Superpower Affiliation { get; set; } = null;

		public Waters TerritorialWaters { get; }

		#region Value Definitions
		public ValueComplex[] Values = new ValueComplex[NUM_VALUES];
		public ConstrainedList<Impact> ExpenditureAndRevenue = new ConstrainedList<Impact>();
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

		public ValueComplex Consumerism
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

		public ObservableCollection<ConstructableObject> ConstructionSequence { get; private set; } 
			= new ObservableCollection<ConstructableObject>();

		private void AddExpenditureAndRevenue(Impact cost)
		{
			for (int i = 0; i < cost.Values.Length; i++)
			{
				// if any of the values of cost is meaningful (i.e. with an actual number), do this
				if (cost.Values[i].Value != 0)
				{
					this.Values[i].Growth.AddValue(cost.Name, cost.Values[i].Value);
				}
			}
		}
		private void RemoveExpenditureAndRevenue(Impact cost)
		{
			for (int i = 0; i < cost.Values.Length; i++)
			{
				// vice versa
				if (cost.Values[i].Value != 0)
				{
					// v.v.
					this.Values[i].Growth.Items[cost.Name] -= cost.Values[i].Value;
					if (this.Values[i].Growth.Items[cost.Name] == 0)
					{
						this.Values[i].Growth.Items.Remove(cost.Name);
					}
				}
			}
		}
		private void ApplyEvent(Event newEvent)
		{

		}

		public void AddCostOfExecution(Impact cost)
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
	public class Superpower : Nation
	{
		public const int INITIAL_NUKE_SILOS = 10;
		public const int NUM_ADJACENT_NATIONS = 5;

		public Superpower() : base()
		{
			for (int i = 0; i < NUM_ADJACENT_NATIONS; i++)
			{
				RegularNation nation = new RegularNation() { Name = Data.NationNames.Dequeue() };
				Data.Regions.Add(nation);
				// initialize the no. of adj nations
				Adjacency[i] = nation;
			}

			Affiliation = this;
		}

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public static Superpower InitializeMe(string name)
		{
			Superpower superpower = new Superpower();

			superpower.Name = name;
			superpower.Economy = new ValueComplex(20000 + 400);  // x10^9
			superpower.HiEduPopu = new ValueComplex(20000);      // x10^6
			superpower.Army = new ValueComplex(50000);           // x10^3
			superpower.Navy = new ValueComplex(5000);            // x10^3
			superpower.Food = new ValueComplex(20000);           // x10^6
			superpower.RawMaterial = new ValueComplex(4000);     // x10^3
			superpower.NuclearMaterial = new ValueComplex(100);  // x10^3
			superpower.Stability = new ValueComplex(75) { Maximum = 100 };

			for (int i = 0; i < INITIAL_NUKE_SILOS; i++)
			{
				superpower.NuclearPlatforms.Add(new Silo(superpower));
			}

			superpower.ExpenditureAndRevenue.Add(
				new Impact(
					"Army Maintenance", 
					new Expression(superpower.Army.CurrentValue, v => -0.001 * v)
					)
				);
			superpower.ExpenditureAndRevenue.Add(
				new Impact(
					"Navy Maintenance", 
					economy: new Expression(superpower.Navy.CurrentValue, v => -0.005 * v)
					)
				);
			superpower.ExpenditureAndRevenue.Add(
				new Impact(
					"Domestic Development", 
					new Expression(superpower.Economy.CurrentValue, v => -0.9 * v)
				)
			);
			superpower.ExpenditureAndRevenue.Add(
				new Impact("Government Revenue", economy: 20000));
			superpower.ExpenditureAndRevenue.Add(
				new Impact("Graduates", hiEduPopu: 1000));
			superpower.ExpenditureAndRevenue.Add(
				new Impact("Domestic Production", food: 42000));
			superpower.ExpenditureAndRevenue.Add(
				new Impact(
					"Domestic Consumption", 
					food: new Expression(superpower.HiEduPopu.CurrentValue, v => -2 * v)
				)
			);
			superpower.ExpenditureAndRevenue.Add(
				new Impact("Domestic Production", rawMaterial: 10200));
			superpower.ExpenditureAndRevenue.Add(
				new Impact("Industrial Consumption", rawMaterial: -10000));
			superpower.ExpenditureAndRevenue.Add(
				new Impact("Production", nuclearMaterial: 1));

			return superpower;
		}
	}
}
