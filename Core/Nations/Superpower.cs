using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System.Collections.Generic;
using System;
using UIEngine;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LCGuidebook.Core
{
	public class Superpower : Nation
	{
		//public const int INITIAL_NUKE_SILOS = 10;
		//public const int NUM_ADJACENT_NATIONS = 5;

		public Superpower(string name) : base(name)
		{
			//for (int i = 0; i < NUM_ADJACENT_NATIONS; i++)
			//{
			//	RegularNation nation = new RegularNation() { Name = ResourceManager.NationNames.Dequeue() };
			//	ResourceManager.Regions.Add(nation);
			//	// initialize the no. of adj nations
			//	Adjacency[i] = nation;
			//}

			//Inclination = this;
		}

		[Visible(nameof(MainCommandGroups))]
		public List<ActionGroup> MainCommandGroups { get; set; }

		public ConstrainedList<Platform> NuclearPlatforms { get; set; } = new ConstrainedList<Platform>();

		//public RegularNation[] Adjacency { get; set; } = new RegularNation[NUM_ADJACENT_NATIONS];

		//public List<RegularNation> SateliteNations { get; set; } = new List<RegularNation>();

		public void SendToReserve(IUniqueObject uniqueObject)
		{
			_Reserve.Add(uniqueObject.UID, uniqueObject);
		}
		public T GetFromReserve<T>(string uid) where T : IUniqueObject
		{
			return (T)_Reserve[uid];
		}

	}
}