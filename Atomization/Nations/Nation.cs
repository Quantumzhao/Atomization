using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

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

			Economy = new ValueComplex(this, 20000);		// x10^9
			HiEduPopu = new ValueComplex(this, 20000);		// x10^6
			Army = new ValueComplex(this, 50000);			// x10^3
			Navy = new ValueComplex(this, 5000);			// x10^3
			Food = new ValueComplex(this, 20000);			// x10^6
			RawMaterial = new ValueComplex(this, 4000);		// x10^3
			NuclearMaterial = new ValueComplex(this, 100);	// x10^3
			Stability = new ValueComplex(this, 75) { Maximum = new ValueComplex.InternalValue(this, 100) };
		}

		public const int NumOfNonAdjacentNations = 5;

		#region Value Definitions
		public Waters TerritorialWaters { get; }
		public ValueComplex Economy { get; set; }
		public ValueComplex HiEduPopu { get; set; }
		public ValueComplex Army { get; set; }
		public ValueComplex Navy { get; set; }
		public ValueComplex Food { get; set; }
		public ValueComplex RawMaterial { get; set; }
		public ValueComplex NuclearMaterial { get; set; }
		public ValueComplex Stability { get; set; }
		#endregion

		public class ValueComplex
		{
			public ValueComplex(Nation parent, double initialValue = 0)
			{
				value = new InternalValue(parent, initialValue);
				Maximum = new InternalValue(parent, double.MaxValue);
				Minimum = new InternalValue(parent, double.MinValue);
				Growth = new Value_Growth(parent, value);
			}

			private InternalValue value;
			public double Value
			{
				get => value.Value;
				set => this.value.Value = value;
			}
			public InternalValue Maximum { get; set; }
			public InternalValue Minimum { get; set; }
			public Value_Growth Growth { get; set; }

			public event OnValueChanged<Nation, double> OnValueChanged
			{
				add => this.value.OnValueChanged += value;
				remove => this.value.OnValueChanged -= value;
			}

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

			public class Value_Growth : INotifyCollectionChanged
			{
				public Value_Growth(Nation parent, InternalValue bindedValue)
				{
					this.parent = parent;
					this.bindedValue = bindedValue;
				}

				private InternalValue bindedValue;
				private Nation parent;

				public event NotifyCollectionChangedEventHandler CollectionChanged;

				public Dictionary<string, InternalValue> Values { get; set; } 
					= new Dictionary<string, InternalValue>();

				public void Add(string name, int number)
				{
					var value = new InternalValue(parent, number);
					value.OnValueChanged += (n, pv, nv) => CollectionChanged?.Invoke(
						this, 
						new NotifyCollectionChangedEventArgs(
							NotifyCollectionChangedAction.Replace, 
							value
						)
					);
					Values.Add(name, value);
					CollectionChanged?.Invoke(
						this, 
						new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value)
					);
				}
				public void Add(string name, double percent)
				{
					Add(name, (int)(bindedValue.Value * percent));
				}

				public int Sum
				{
					get
					{
						double sum = 0;
						foreach (var item in Values)
						{
							sum += item.Value.Value;
						}
						return (int)sum;
					}
				}
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
		public Superpower(NotifyCollectionChangedEventHandler onCollectionChanged = null) : base()
		{
			for (int i = 0; i < InitialNukeSilos; i++)
			{
				NuclearPlatforms.Add(new Silo(onCollectionChanged) { DeployRegion = this});
			}

			for (int i = 0; i < NumOfAdjacentNations; i++)
			{
				RegularNation nation = new RegularNation() { Name = Data.NationNames.Dequeue() };
				Data.Regions.Add(nation);
				Adjacency[i] = nation;
			}

			Economy.Growth.Add("Army Maintenance", (int)(-0.001 * Army.Value));
			Economy.Growth.Add("Navy Maintenance Cost", (int)(-0.005 * Navy.Value));
			Economy.Growth.Add("Domestic Development", -0.9);
			Economy.Growth.Add("Government Revenue", 20000);

			HiEduPopu.Growth.Add("Graduates", 1000);

			Army.Growth.Add("Army Expansion", 0.01);

			Navy.Growth.Add("Navy Expansion", 0.005);

			Food.Growth.Add("Domestic Production", 42000);
			Food.Growth.Add("Domestic Consumption", -2.0);

			RawMaterial.Growth.Add("Domestic Production", 10200);
			RawMaterial.Growth.Add("Industrial Consumption", -10000);

			NuclearMaterial.Growth.Add("Production", 1);
		}
		public const int NumOfAdjacentNations = 5;

		public GameObjectList<Platform> NuclearPlatforms { get; set; } = new GameObjectList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NumOfAdjacentNations];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();
	}
}
