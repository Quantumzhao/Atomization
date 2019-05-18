using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public abstract class Nation
	{
		public const int NumOfNonAdjacentNations = 5;

		#region Fund
		public const int InitialEcon = 1000000;
		public event Action<Nation, int, int> OnFundChanged;
		private int econ = InitialEcon;
		public int Econ
		{
			get => econ;
			set
			{
				OnFundChanged(this, econ, value);
				econ = value;
			}
		}
		public delegate void One(Nation nation, int a, int b);
		public event One OnEconGrowthChanged;
		private List<int> econGrowth = new List<int>();
		public List<int> EconGrowth
		{
			get => econGrowth;
			set
			{
				//OnEconGrowthChanged()
				econGrowth = value;
			}
		}
		#endregion

		#region HighEducationPopulation
		public const int InitialHiEduPopu = 10000000;
		public int HiEduPopu { get; set; } = InitialHiEduPopu;
		public List<int> HiEduPopuGrowth { get; set; } = new List<int>();
		#endregion

		#region RegularMilitary
		public const int InitialRegularMilitary = 500000;
		public int RegularMilitary { get; set; } = InitialRegularMilitary;
		public List<int> RegularMilitaryGrowth { get; set; } = new List<int>();
		#endregion

		#region Resource
		public const int InitialResource = 10000;
		public int Resource { get; set; } = InitialResource;
		public List<int> ResourceGrowth { get; set; } = new List<int>();
		#endregion

		#region Stability
		public const int InitialStability = 100;
		public int MaxStability { get; set; } = InitialStability;
		public int Stability { get; set; } = InitialStability;
		public double StabilityPercent => (double)Stability / MaxStability;
		public List<int> StabilityGrowth = new List<int>();
		#endregion
	}

	public class RegularNation : Nation
	{
		public RegularNation()
		{
		}

		// null stands for independence
		public Superpower Sponsor { get; set; } = null;
	}
	public class Superpower : Nation
	{
		public string Name { get; set; }

		public const int InitialNukeSilos = 10;
		public Superpower() : base()
		{
			for (int i = 0; i < InitialNukeSilos; i++)
			{
				NuclearPlatforms.Add(new Silo());
			}
		}
		public const int NumOfAdjacentNations = 5;

		public List<Platform> NuclearPlatforms { get; set; } = new List<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NumOfAdjacentNations];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public event Action<object, EventArgs> OnChanged;
	}
}
