using Atomization.DataStructures;
using System;

namespace Atomization
{
	public abstract class Warhead : IDestroyable
	{
		public virtual string WarheadType { get; } = "Warh";

		public event Action SelfDestroyed;

		public void DestroyThis() => SelfDestroyed?.Invoke();
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
