using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using LCGuidebook.Core.DataStructures;
using UIEngine;

namespace LCGuidebook.Core
{
	public static class ResourceManager
	{
		public static List<Region> Regions = new List<Region>();
		public static Queue<string> WatersNames { get; private set; }
		public static Queue<string> NationNames { get; private set; }

		[Visible(nameof(Me))]
		public static Superpower Me { get; set; }

		public static ObservableCollection<NuclearWeapon> MyNuclearWeapons { get; set; }
			= new ObservableCollection<NuclearWeapon>();

		private static readonly Dictionary<string, Dictionary<TypesOfCostOfStage, CostOfStage>> CostTable 
			= new Dictionary<string, Dictionary<TypesOfCostOfStage, CostOfStage>>();

		public static void Initialize()
		{
			#region Initialize names of regions
			try
			{
				WatersNames = new Queue<string>(File.ReadAllLines($"{Misc.SolutionPath}/Core/Nations/Waters Names.txt"));
			}
			catch (Exception)
			{
				for (int i = 0; i < 25; i++)
				{
					WatersNames.Enqueue("[Error]");
				}
			}
			try
			{
				NationNames = new Queue<string>(File.ReadAllLines($"{Misc.SolutionPath}/Core/Nations/Nation Names.txt"));
			}
			catch (Exception)
			{
				for (int i = 0; i < 20; i++)
				{
					NationNames.Enqueue("[Error]");
				}
			}
			#endregion

			Misc.Initialize();
		}

		public static void RegisterCost(Type type, CostOfStage manufacture, CostOfStage deployment, 
			CostOfStage transportation, CostOfStage maintenance)
		{
			var costs = new Dictionary<TypesOfCostOfStage, CostOfStage>();
			costs.Add(TypesOfCostOfStage.Manufacture, manufacture);
			costs.Add(TypesOfCostOfStage.Deployment, deployment);
			costs.Add(TypesOfCostOfStage.Transportation, transportation);
			costs.Add(TypesOfCostOfStage.Maintenance, maintenance);
			CostTable.Add(type.ToString(), costs);
		}

		public static class Misc
		{
			public static void Initialize()
			{
				NukeDisposal = new CostOfStage(
					longTermEffect: new Effect(),
					shortTermEffect: new Effect(),
					requiredTime: (Expression)0					
				);
			}

			public static CostOfStage NukeDisposal { get; private set; }
			public static string SolutionPath { get; set; }
		}
	}
}
