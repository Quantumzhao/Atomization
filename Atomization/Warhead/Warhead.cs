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
