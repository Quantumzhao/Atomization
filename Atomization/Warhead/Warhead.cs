using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization
{
	public abstract class Warhead
	{
		public virtual string WarheadType { get; } = "Warh";
	}
	public class Atomic : Warhead
	{
		public override string WarheadType => "Atomic";
	}
	public class Hydrogen : Warhead
	{
		public override string WarheadType => "Hydrogen";
	}
	public class Dirty : Warhead
	{
		public override string WarheadType => "Dirty";
	}
}
