using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atomization
{
	/// <summary>
	/// Interaction logic for Nuclear.xaml
	/// </summary>
	public partial class Nuclear : Page
	{
		public DeployNuke_Window DeployNuke_Window = null;

		public Nuclear()
		{
			InitializeComponent();

			NukeList.ItemsSource = Data.MyNuclearWeapons;			
		}

		private void Button_DeployNuke_Click(object sender, RoutedEventArgs e)
		{
			if (DeployNuke_Window == null)
			{
				DeployNuke_Window = new DeployNuke_Window(this);
				DeployNuke_Window.Show();
			}
			else
			{
				DeployNuke_Window.Focus();
			}
		}

		private void Button_DisposeNuke_Click(object sender, RoutedEventArgs e)
		{
			NuclearWeapon selectedWeapon = NukeList.SelectedItem as NuclearWeapon;
			if (selectedWeapon == null)
			{
				MessageBox.Show(
					"Please select one first",
					"Information",
					MessageBoxButton.OK,
					MessageBoxImage.Information);
			}
			else
			{
				var result = MessageBox.Show(
					"Are you sure to dispose the selected nuclear weapon?",
					"Confirmation",
					MessageBoxButton.OKCancel,
					MessageBoxImage.Warning);
				switch (result)
				{
					case MessageBoxResult.OK:
						selectedWeapon.Platform.NuclearWeapons.Remove(selectedWeapon);
						break;
					default:
						break;
				}
			}
		}

		private void NewPlatform_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			switch ((NewPlatform.SelectedItem as ComboBoxItem)?.Content.ToString())
			{
				case "Silo":
				case "Missile Launcher":
				case "Strategic Bomber":
					RegionOfDeployment.ItemsSource = new List<Nation>(Data.Me.SateliteNations) { Data.Me };
					break;

				case "Nuclear Submarine":
					RegionOfDeployment.ItemsSource = new List<Region>(Data.Regions.Where(r => r is Waters));
					break;

				default:
					return;
			}
		}

		private void Button_Deploy_Click(object sender, RoutedEventArgs e)
		{
			Platform prefab;
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> addHandler =
				(list, item) => Data.MyNuclearWeapons.Add(item);
			Action<GameObjectList<NuclearWeapon>, NuclearWeapon> RemoveHandler =
				(list, item) => Data.MyNuclearWeapons.Remove(item);

			switch ((NewPlatform.SelectedItem as ComboBoxItem)?.Content.ToString())
			{
				case "Silo":
					prefab = new Silo(
						onItemAdded: addHandler,
						onItemRemoved: RemoveHandler
					);
					break;

				case "Strategic Bomber":
					prefab = new StrategicBomber(
						onItemAdded: addHandler,
						onItemRemoved: RemoveHandler
					);
					break;

				case "Missile Launcher":
					prefab = new MissileLauncher(
						onItemAdded: addHandler,
						onItemRemoved: RemoveHandler
					);
					break;

				case "Nuclear Submarine":
					prefab = new NuclearSubmarine(
						onItemAdded: addHandler,
						onItemRemoved: RemoveHandler
					);
					break;

				default:
					return;
			}

			if (RegionOfDeployment.SelectedItem == null) return;
			prefab.DeployRegion = RegionOfDeployment.SelectedItem as Region;
			Data.Me.NuclearPlatforms.Add(prefab);

			MessageBox.Show(
				"New nuclear strike platform deployed successfully", 
				"Intelligence", 
				MessageBoxButton.OK, 
				MessageBoxImage.Information);
		}
	}
}
