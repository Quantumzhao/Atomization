using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization.DataStructures
{
	// The class for all events (aka acts)
	public class Event : ConstructableObject
	{
		public string Name { get; set; }

		public override string TypeName => nameof(Event);
	}
}
