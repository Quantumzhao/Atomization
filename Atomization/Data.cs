using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public static class Data
	{
		static Data()
		{
			#region Initialize Myself
			Superpowers.Add(new Superpower() { Name = "C" });
			Me = Superpowers[0];
			#endregion

			#region Initialize Opponents
			// Add only one opponent to the board for current stage
			Superpowers.Add(new Superpower() { Name = "A"});
			#endregion

			#region Initialize Regular Nation
			// Initialize Adjacent nations
			foreach (var superpower in Superpowers)
			{
				RegularNation nation = new RegularNation();
				for (int i = 0; i < Superpower.NumOfAdjacentNations; i++)
				{
					superpower.Adjacency[i] = nation;
					RegularNations.Add(nation);
				}
			}
			// Initialize non-adjacent nations
			for (int i = 0; i < Nation.NumOfNonAdjacentNations; i++)
			{
				RegularNations.Add(new RegularNation());
			}
			#endregion
		}
		public static List<RegularNation> RegularNations { get; set; } = new List<RegularNation>();
		public static List<Superpower> Superpowers { get; set; } = new List<Superpower>();
		public static Superpower Me { get; set; }
	}
}
