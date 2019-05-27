using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Atomization
{
	public static class Data
	{
		public static List<Region> Regions = new List<Region>();
		public static Queue<string> WatersNames { get; private set; }
		public static Queue<string> NationNames { get; private set; }

		public static Superpower Me;

		public static ObservableCollection<NuclearWeapon> MyNuclearWeapons { get; set; }
			= new ObservableCollection<NuclearWeapon>();

		public static void Initiaze()
		{
			try
			{
				WatersNames = new Queue<string>(File.ReadAllLines("..\\..\\Nations\\Waters Names.txt"));
			}
			catch (Exception)
			{
				for (int i = 0; i < 25; i++)
				{
					WatersNames.Enqueue("");
				}
			}
			try
			{
				NationNames = new Queue<string>(File.ReadAllLines("..\\..\\Nations\\Nation Names.txt"));
			}
			catch (Exception)
			{
				for (int i = 0; i < 20; i++)
				{
					NationNames.Enqueue("");
				}
			}

			Regions.Add(
				Me = new Superpower(
					onItemAdded: (list, item) => MyNuclearWeapons.Add(item), 
					onItemRemoved: (list, item) => MyNuclearWeapons.Remove(item)
				) { Name = "C"}
			);
			Regions.Add(new Superpower() { Name = "A" });
		}
	}
}
