using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

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
					(sender, e) =>
					{
						if (e.Action == NotifyCollectionChangedAction.Add)
						{
							MyNuclearWeapons.Add(e.NewItems[0] as NuclearWeapon);
						}
						else if (e.Action == NotifyCollectionChangedAction.Remove)
						{
							MyNuclearWeapons.Remove(e.NewItems[0] as NuclearWeapon);
						}
					}
				)
				{ Name = "C" }
			);
			Regions.Add(new Superpower() { Name = "A" });
		}
	}
}
