using LCGuidebook.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCGuidebook.Core
{
	public class NuclearMissile : IDeployable
	{
		public int Damage { get; set; }
		public bool IsActivated { get; set; }
		public Region DeployedRegion { get; set; }

		public string UID { get; } = GameManager.GenerateUID();

		public event Action SelfDestroyed;

		public void DestroyThis()
		{
			SelfDestroyed?.Invoke();
		}
	}
}
