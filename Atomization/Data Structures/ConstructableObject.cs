using System.ComponentModel;

namespace Atomization
{
	public abstract class ConstructableObject : IBuild
	{
		public event ConstructionCompleted ConstructionCompleted;

		protected VM<int> buildTime;
		public int BuildTime
		{
			get => buildTime.ObjectData;
			set
			{
				if (value == 0)
				{
					ConstructionCompleted?.Invoke(this);
					buildTime.ObjectData = -1;
					return;
				}
				else if (buildTime.ObjectData == -1)
				{
					return;
				}

				if (value != buildTime.ObjectData)
				{
					buildTime.ObjectData = value;
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