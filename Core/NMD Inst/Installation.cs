using LCGuidebook.Core.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCGuidebook.Core
{
	public class Installation : IDeployable
	{
		public bool IsActivated { get; set; }
		public Region DeployedRegion { get; set; }
		public string UID { get; } = GameManager.GenerateUID();

		private double _SuccessfulInterceptionRate;
		public double SuccessfulInterceptionRate 
		{ 
			get => _SuccessfulInterceptionRate;
			set => value.Clamp();
		}

		public event Action SelfDestroyed;

		public void DestroyThis()
		{
			SelfDestroyed?.Invoke();
		}
	}
}
