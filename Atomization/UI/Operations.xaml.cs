using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atomization
{	
	public partial class Operations : Page
	{
		public ObservableCollection<Region> waters => new ObservableCollection<Region>(Data.Regions.Where(r => r is Waters));

		public Operations()
		{
			InitializeComponent();

			Expander_Maneuver.Expanded += LoadNumNukeListBox;

			SelectionList.ItemsSource = waters;
		}

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

		private void LoadNumNukeListBox(object sender, RoutedEventArgs e)
		{
			foreach (var platform in Data.Me.NuclearPlatforms)
			{
				foreach (var weapon in platform.NuclearWeapons)
				{
					Maneuver_NumNukes.Items.Add(weapon.Name);
				}
			}
		}
		private void UnloadNumNukeListBox(object sender, RoutedEventArgs e)
		{
			Maneuver_NumNukes.Items.Clear();
		}
	}
}
