using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;

namespace LCGuidebook.Core
{
	//public delegate void StageFinishedHandler(Task previous, Task next, Task task);

	public static class Misc
	{
		public const string BAD_ACTIVATION_SEQUENCE = "Must be activated after all dependencies";

		public static int Round(double value) => (int)(value + 0.5);

		public static LinkedList<Region> ShortestPath(Region start, Region end)
		{
			// Assume all weight between nodes are 1 for now
			// (node, predecessor, distance, isOutside)
			var table = ResourceManager.Regions.Select<Region, (Region, Region, int, bool)>(r => 
			{ 
				if (r == start) return (r, null, int.MaxValue, true); 
				else return (r, null, 0, true); }
			).ToList();
			while (table.Count != 0)
			{
				// find the tuple with least distance
				var tup = table.Aggregate((acc, t) => {
					if (acc.Item3 > t.Item3)
					{
						return t;
					}
					else
					{
						return acc;
					}
				});
				tup.Item4 = false;
				tup.Item1.Neighbors.Aggregate((acc, n) =>
				{
					var t = table.Find(r => r.Item1 == n);
					if (tup.Item3 + 1 < t.Item3)
					{
						tup.Item3++;
						t.Item2 = tup.Item1;
					}
					return acc;
				});
			}
			var path = new LinkedList<Region>();
			path.AddFirst(end);
			var iter = table.Find(t => t.Item1 == end);
			while (iter.Item2 != null)
			{
				path.AddFirst(iter.Item2);
				iter = table.Find(t => t.Item1 == iter.Item2);
			}
			return path;
		}
	}
}
