using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.Collections.Generic;
using System;
using UIEngine;

namespace LCGuidebook.Core
{
	public class Superpower : Nation
	{
		public const int INITIAL_NUKE_SILOS = 10;
		public const int NUM_ADJACENT_NATIONS = 5;

		public Superpower() : base()
		{
			for (int i = 0; i < NUM_ADJACENT_NATIONS; i++)
			{
				RegularNation nation = new RegularNation() { Name = ResourceManager.NationNames.Dequeue() };
				ResourceManager.Regions.Add(nation);
				// initialize the no. of adj nations
				Adjacency[i] = nation;
			}

			Affiliation = this;
		}

		[Visible(nameof(MainCommandGroups))]
		public List<CommandGroup> MainCommandGroups { get; set; }

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public void SendToReserve(IUniqueObject uniqueObject)
		{
			_Reserve.Add(uniqueObject.UID, uniqueObject);
		}
		public T GetFromReserve<T>(string uid) where T : IUniqueObject
		{
			return (T)_Reserve[uid];
		}

		#region Temp holder for static commands
		//public static void DeployNewNuclearStrikePlatform(Platform.Types type, Region region)
		//{
		//	Data.Me.TaskSequence.AddNewTask(Task.Types.MTD, $"Deploying a new {type}");
		//	throw new NotImplementedException();
		//}

		public static void DestroyNuke(NuclearWeapon nuclearWeapon)
		{
			Deployment destruction = new Deployment($"Destroying {nuclearWeapon}", null, nuclearWeapon,
				ResourceManager.Misc.NukeDisposal);
			ResourceManager.Me.TaskSequence.AddNewTask(destruction);

			EventManager.TaskProgressAdvenced += (s, e) => RemoveNuke(s, e, nuclearWeapon);
		}
		static void RemoveNuke(Task sender, TaskProgressAdvancedEventArgs e, NuclearWeapon weapon)
		{
			if (e.IsTaskFinished &&
				sender is Deployment deployment &&
				deployment.DeployableObject == weapon)
			{
				weapon.DestroyThis();
			}
		}

		public static void DeployNewNuclearWeapon(Platform.Types platformType, 
			Warhead.Types warheadType, CarrierType carrierType, Region target)
		{
			Manufacture manufacture;

			switch (warheadType)
			{
				case Warhead.Types.AtomicBomb:
					break;
				case Warhead.Types.HydrogenBomb:
					break;
				case Warhead.Types.DirtyBomb:
					break;
				default:
					throw new ArgumentException();
			}
		}

		#endregion

		public enum PublicFigures
		{

		}
	}
}