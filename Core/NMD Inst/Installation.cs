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
			set
			{
				if (value < 0)
				{
					_SuccessfulInterceptionRate = 0;
				}
				else if (value > 1)
				{
					_SuccessfulInterceptionRate = 1;
				}
			}
		}

		public event Action SelfDestroyed;

		public void DestroyThis()
		{
			SelfDestroyed?.Invoke();
		}
	}
}
