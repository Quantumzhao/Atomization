using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public abstract class Region
	{
		public string Name { get; set; }
	}

	public class Waters : Region
	{
		public const int NumOfInternationalWaters = 5;
		public Nation Affiliation { get; set; }
	}

	public abstract class Nation : Region
	{
		public Nation()
		{
			TerritorialWaters = new Waters() { Name = Data.WatersNames.Dequeue() };
			TerritorialWaters.Affiliation = this;
			Data.Regions.Add(TerritorialWaters);
		}

		public const int NumOfNonAdjacentNations = 5;

		public Waters TerritorialWaters { get; }

		public const int InitialEcon = 1000000;
		#region Econ
		public event OnValueChanged<Nation, int> OnEconChanged;
		private int econ = InitialEcon;
		public int Econ
		{
			get => econ;
			set
			{
				if (value != econ)
				{
					OnEconChanged?.Invoke(this, econ, value);
					econ = value;
				}
			}
		}
		#endregion
		#region EconGrowth
		private List<int> econGrowth = new List<int>();
		public event OnValueChanged<Nation, List<int>> OnEconGrowthChanged;
		public List<int> EconGrowth
		{
			get => econGrowth;
			set
			{
				if (value != econGrowth)
				{
					OnEconGrowthChanged?.Invoke(this, econGrowth, value);
					econGrowth = value;
				}
			}
		}
		#endregion

		public const int InitialHiEduPopu = 10000000;
		#region HighEducationPopulation
		public event OnValueChanged<Nation, int> OnHiEduPopuChanged;
		private int hiEduPopu = InitialHiEduPopu;
		public int HiEduPopu
		{
			get => hiEduPopu;
			set
			{
				if (value != hiEduPopu)
				{
					OnHiEduPopuChanged?.Invoke(this, hiEduPopu, value);
					hiEduPopu = value;
				}
			}
		}
		#endregion
		#region HighEducationPopulationGrowth
		public event OnValueChanged<Nation, List<int>> OnHiEduPopuGrowthChanged;
		private List<int> hiEduPopuGrowth = new List<int>();
		public List<int> HiEduPopuGrowth
		{
			get => hiEduPopuGrowth;
			set
			{
				if (value != hiEduPopuGrowth)
				{
					OnHiEduPopuGrowthChanged?.Invoke(this, hiEduPopuGrowth, value);
					hiEduPopuGrowth = value;
				}
			}
		}
		#endregion

		public const int InitialRegularMilitary = 500000;
		#region RegularMilitary
		public event OnValueChanged<Nation, int> OnRegularMilitaryChanged;
		private int regularMilitary = InitialRegularMilitary;
		public int RegularMilitary
		{
			get => regularMilitary;
			set
			{
				if (value != regularMilitary)
				{
					OnRegularMilitaryChanged?.Invoke(this, regularMilitary, value);
					regularMilitary = value;
				}
			}
		}
		#endregion
		#region RegularMilitaryGrowth
		public event OnValueChanged<Nation, List<int>> OnRegularMilitaryGrowthChanged;
		private List<int> regularMilitaryGrowth = new List<int>();
		public List<int> RegularMilitaryGrowth
		{
			get => regularMilitaryGrowth;
			set
			{
				if (value != regularMilitaryGrowth)
				{
					OnRegularMilitaryGrowthChanged?.Invoke(this, regularMilitaryGrowth, value);
					regularMilitaryGrowth = value;
				}
			}
		}
		#endregion

		public const int InitialNavy = 50000;
		#region RegularMilitary
		public event OnValueChanged<Nation, int> OnNavyChanged;
		private int navy = InitialNavy;
		public int Navy
		{
			get => navy;
			set
			{
				if (value != navy)
				{
					OnNavyChanged?.Invoke(this, navy, value);
					navy = value;
				}
			}
		}
		#endregion
		#region RegularMilitaryGrowth
		public event OnValueChanged<Nation, List<int>> OnNavyGrowthChanged;
		private List<int> navyGrowth = new List<int>();
		public List<int> NavyGrowth
		{
			get => navyGrowth;
			set
			{
				if (value != navyGrowth)
				{
					OnNavyGrowthChanged?.Invoke(this, navyGrowth, value);
					navyGrowth = value;
				}
			}
		}
		#endregion

		public const int InitialResource = 10000;
		#region Resource
		public event OnValueChanged<Nation, int> OnResourceChanged;
		private int resource = InitialResource;
		public int Resource
		{
			get => resource;
			set
			{
				if (value != resource)
				{
					OnResourceChanged?.Invoke(this, resource, value);
					resource = value;
				}
			}
		}
		#endregion
		#region ResourceGrowth
		public event OnValueChanged<Nation, List<int>> OnResourceGrowthChanged;
		private List<int> resourceGrowth = new List<int>();
		public List<int> ResourceGrowth
		{
			get => resourceGrowth;
			set
			{
				if (value != resourceGrowth)
				{
					OnResourceGrowthChanged?.Invoke(this, resourceGrowth, value);
					resourceGrowth = value;
				}
			}
		}
		#endregion

		public const int InitialStability = 100;
		#region MaxStability
		public event OnValueChanged<Nation, int> OnMaxStabilityChanged;
		private int maxStability = InitialStability;
		public int MaxStability
		{
			get => maxStability;
			set
			{
				if (value != maxStability)
				{
					OnMaxStabilityChanged?.Invoke(this, maxStability, value);
					maxStability = value;
				}
			}
		}
		#endregion
		#region Stability
		public event OnValueChanged<Nation, int> OnStabilityChanged;
		private int stability = InitialStability;
		public int Stability
		{
			get => stability;
			set
			{
				if (value != stability)
				{
					OnStabilityChanged?.Invoke(this, stability, value);
					stability = value;
				}
			}
		}
		#endregion
		#region StabilityGrowth
		public double StabilityPercent => (double)Stability / MaxStability;
		public event OnValueChanged<Nation, List<int>> OnStabilityGrowthChanged;
		private List<int> stabilityGrowth = new List<int>();
		public List<int> StabilityGrowth
		{
			get => stabilityGrowth;
			set
			{
				if (value != stabilityGrowth)
				{
					OnStabilityGrowthChanged?.Invoke(this, stabilityGrowth, value);
					stabilityGrowth = value;
				}
			}
		}
		#endregion
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
		}

		// null stands for independence
		public Superpower Affiliation { get; set; } = null;
	}
	public class Superpower : Nation
	{
		public const int InitialNukeSilos = 10;
		public Superpower(
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemAdded = null, 
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> onItemRemoved = null
		) : base()
		{
			for (int i = 0; i < InitialNukeSilos; i++)
			{
				NuclearPlatforms.Add(new Silo(onItemAdded, onItemRemoved) { DeployRegion = this});
			}

			for (int i = 0; i < NumOfAdjacentNations; i++)
			{
				RegularNation nation = new RegularNation() { Name = Data.NationNames.Dequeue() };
				Data.Regions.Add(nation);
				Adjacency[i] = nation;
			}
		}
		public const int NumOfAdjacentNations = 5;

		public GameObjectList<Platform> NuclearPlatforms { get; set; } = new GameObjectList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NumOfAdjacentNations];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();
	}
}
