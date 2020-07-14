using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LCGuidebook.Core.DataStructures;
using UIEngine;

namespace LCGuidebook.Core
{
	public static class ResourceManager
	{
		public static List<Region> Regions = new List<Region>();
		//public static Queue<string> WatersNames { get; private set; }
		//public static Queue<string> NationNames { get; private set; }

		private static readonly Dictionary<string, Dictionary<TypesOfCostOfStage, CostOfStage>> CostTable 
			= new Dictionary<string, Dictionary<TypesOfCostOfStage, CostOfStage>>();

		public static ObservableCollection<NuclearWeapon> MyNuclearWeapons { get; set; }
			= new ObservableCollection<NuclearWeapon>();

		[Visible(nameof(Me))]
		public static Superpower Me { get; set; }

		private static int _NumOfFigures = -1;
		public static int NumOfFigures
		{
			get => _NumOfFigures;
			set
			{
				if (_NumOfFigures == -1)
				{
					_NumOfFigures = value;
				}
				else
				{
					throw new InvalidOperationException();
				}
			}
		}

		//public static void Initialize()
		//{
		//	#region Initialize names of regions
		//	try
		//	{
		//		WatersNames = new Queue<string>(File.ReadAllLines(
		//			$"{Misc.SolutionPath}/Core/Nations/Waters Names.txt"));
		//	}
		//	catch (Exception)
		//	{
		//		for (int i = 0; i < 25; i++)
		//		{
		//			WatersNames.Enqueue("[Error]");
		//		}
		//	}
		//	try
		//	{
		//		NationNames = new Queue<string>(File.ReadAllLines(
		//			$"{Misc.SolutionPath}/Core/Nations/Nation Names.txt"));
		//	}
		//	catch (Exception)
		//	{
		//		for (int i = 0; i < 20; i++)
		//		{
		//			NationNames.Enqueue("[Error]");
		//		}
		//	}
		//	#endregion

		//	Misc.Initialize();
		//}

		public static void RegisterCost(string typeOfGameObject, TypesOfCostOfStage costType, 
			CostOfStage cost)
		{
			Dictionary<TypesOfCostOfStage, CostOfStage> costs;
			if (CostTable.ContainsKey(typeOfGameObject.ToString()))
			{
				costs = CostTable[typeOfGameObject];
			}
			else
			{
				costs = new Dictionary<TypesOfCostOfStage, CostOfStage>();
				CostTable.Add(typeOfGameObject.ToString(), costs);
			}

			if (!costs.ContainsKey(costType))
			{
				costs.Add(costType, cost);
			}
		}
		public static CostOfStage GetCostOf(string entityName, TypesOfCostOfStage type)
		{
			return CostTable[entityName][type];
		}

		public static class Misc
		{
			public static void Initialize()
			{
				NukeDisposal = new CostOfStage(
					longTermEffect: new Effect(new Expression[NumOfFigures]),
					shortTermEffect: new Effect(new Expression[NumOfFigures]),
					requiredTime: (Expression)0					
				);
			}

			public static CostOfStage NukeDisposal { get; private set; }
			public static string SolutionPath { get; set; }
		}
	}
}
