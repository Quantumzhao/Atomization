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

			Economy = new ValueComplex(this, 1000000);
			HiEduPopu = new ValueComplex(this, 10000000);
			Army = new ValueComplex(this, 500000);
			Navy = new ValueComplex(this, 50000);
			Food = new ValueComplex(this, 5000);
			RawMaterial = new ValueComplex(this, 4000);
			NuclearMaterial = new ValueComplex(this, 1000);
			Stability = new ValueComplex(this, 100) { Maximum = new ValueComplex.InternalValue(this, 100) };
		}

		public const int NumOfNonAdjacentNations = 5;

		public Waters TerritorialWaters { get; }

		public ValueComplex Economy { get; set; }
		public ValueComplex HiEduPopu { get; set; }
		public ValueComplex Army { get; set; }
		public ValueComplex Navy { get; set; }
		public ValueComplex Food { get; set; }
		public ValueComplex RawMaterial { get; set; }
		public ValueComplex NuclearMaterial { get; set; }
		public ValueComplex Stability { get; set; }

		public class ValueComplex
		{
			public ValueComplex(Nation parent, double initialValue = 0)
			{
				Value = new InternalValue(parent, initialValue);
				Maximum = new InternalValue(parent, double.MaxValue);
				Minimum = new InternalValue(parent, double.MinValue);
				Growth = new Value_Growth(parent);
			}

			public InternalValue Value { get; set; }
			public InternalValue Maximum { get; set; }
			public InternalValue Minimum { get; set; }
			public Value_Growth Growth { get; set; }

			public class InternalValue
			{
				public InternalValue(Nation parent, double initialValue)
				{
					this.parent = parent;
					value = initialValue;
				}

				private Nation parent;
				public event OnValueChanged<Nation, double> OnValueChanged;

				private double value;
				public double Value
				{
					get => value;
					set
					{
						if (value != this.value)
						{
							OnValueChanged?.Invoke(parent, this.value, value);
							this.value = value;
						}
					}
				}
			}

			public class Value_Growth
			{
				public Value_Growth(Nation parent)
				{
					this.parent = parent;
				}

				private Nation parent;
				public Dictionary<string, InternalValue> Values { get; set; } 
					= new Dictionary<string, InternalValue>();
			}
		}
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

			Economy.Growth.Values.Add("Military Budget (Army)", new ValueComplex.InternalValue(this, 5));
		}
		public const int NumOfAdjacentNations = 5;

		public GameObjectList<Platform> NuclearPlatforms { get; set; } = new GameObjectList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NumOfAdjacentNations];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();
	}
}
