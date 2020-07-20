using LCGuidebook.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCGuidebook.Core
{
	public class NuclearMissile : IDeployable
	{
		public NuclearMissile(Nation deployedRegion)
		{
			DeployedRegion = deployedRegion;
		}

		public int Damage { get; set; } = 50;
		public bool IsActivated { get; set; }
		public Region DeployedRegion { get; set; }
		public Nation Target { get; set; }
		public Region Position { get; set; }

		public string UID { get; } = GameManager.GenerateUID();

		public event Action SelfDestroyed;

		public void DestroyThis()
		{
			SelfDestroyed?.Invoke();
		}
	}
}
