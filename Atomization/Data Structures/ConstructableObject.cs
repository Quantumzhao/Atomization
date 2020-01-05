using System.ComponentModel;
using Atomization;
using Atomization.DataStructures;

namespace Atomization.DataStructures
{
	public abstract class ConstructableObject : IBuildable
	{
		public event ConstructionCompletedHandler ConstructionCompleted;

		protected int _BuildTime;
		public int BuildTime
		{
			get => _BuildTime;
			set
			{
				if (value == 0)
				{
					ConstructionCompleted?.Invoke(this);
					_BuildTime = -1;
					return;
				}
				else if (_BuildTime == -1)
				{
					return;
				}

				if (value != _BuildTime)
				{
					_BuildTime = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuildTime)));
				}
			}
		}

		protected Impact _DirectImpact;
		public Impact DirectImpact
		{
			get => _DirectImpact;
			set
			{
				if (value != _DirectImpact)
				{
					_DirectImpact = value;
					_DirectImpact.PropertyChanged += (sender, e) =>
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectImpact)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DirectImpact)));
				}
			}
		}

		protected Impact _LongTermImpact;
		public Impact LongTermImpact
		{
			get => _LongTermImpact;
			set
			{
				if (value != _LongTermImpact)
				{
					_LongTermImpact = value;
					_LongTermImpact.PropertyChanged += (sender, e) =>
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_LongTermImpact)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_LongTermImpact)));
				}
			}
		}

		//public bool IsMine { get; set; } = true;
		public abstract string TypeName { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}
	}
}