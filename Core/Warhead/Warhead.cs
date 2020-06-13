using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using System;

namespace LCGuidebook.Core
{
	public abstract class Warhead : IDestroyable
	{
		public string UID { get; } = GameManager.GenerateUID();

		public event Action SelfDestroyed;

		public void DestroyThis() => SelfDestroyed?.Invoke();

		public enum Types
		{
			AtomicBomb, 
			HydrogenBomb, 
			DirtyBomb
		}
	}
	public class Atomic : Warhead
	{
	}
	public class Hydrogen : Warhead
	{
	}
	public class Dirty : Warhead
	{
	}
}
