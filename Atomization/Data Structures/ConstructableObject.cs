using System.ComponentModel;

namespace Atomization
{
	public abstract class ConstructableObject : IBuild
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

		protected Cost buildCost;
		public Cost BuildCost
		{
			get => buildCost;
			set
			{
				if (value != buildCost)
				{
					buildCost = value;
					buildCost.PropertyChanged += (sender, e) =>
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuildCost)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BuildCost)));
				}
			}
		}

		protected Cost maintenance;
		public Cost Maintenance
		{
			get => maintenance;
			set
			{
				if (value != maintenance)
				{
					maintenance = value;
					maintenance.PropertyChanged += (sender, e) =>
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(maintenance)));
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(maintenance)));
				}
			}
		}

		public bool IsMine { get; set; } = true;
		public abstract string TypeName { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}
	}
}