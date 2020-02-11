using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Atomization
{
	public partial class Operations : Page
	{
		public ObservableCollection<Region> waters =>
			new ObservableCollection<Region>(ResourceManager.Regions.Where(r => r is Waters));

		public Operations()
		{
			InitializeComponent();

			SelectionList.ItemsSource = waters;

			Target.ItemsSource = ResourceManager.Regions;

			//Maneuver_NumNukes.ItemsSource = new ObservableCollection<VM<NuclearWeapon>>(
			//	Data.MyNuclearWeapons.Where(w => !(w.ObjectData.Platform is Silo))
			//);
		}

		#region Toggle Expander Appearance
		private void Expander_Expanded(object sender, RoutedEventArgs e)
		{
			(sender as Expander).Effect = new DropShadowEffect()
			{
				Direction = 0,
				BlurRadius = 10,
				ShadowDepth = 0,
				Opacity = 0.5,
				Color = Colors.Black
			};
		}
		private void Expander_Collapsed(object sender, RoutedEventArgs e)
		{
			(sender as Expander).Effect = null;
		}

		#endregion

		private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
		{
			Target.IsEnabled = false;
		}

		private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
		{
			Target.IsEnabled = true;
		}
	}
}
