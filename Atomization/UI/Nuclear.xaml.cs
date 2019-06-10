using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
					Deploy_ToolTip.Items.Add(new TextBlock() { Text = "Building Silo Takes 4 turns" });
					Deploy_ToolTip.Items.Add(new TextBlock() { Text = "Building Silo Costs 4 turns" });

					goto setLandBase;

				case "Missile Launcher":
					Deploy_ToolTip.Items.Add(new TextBlock() { Text = "Building Missile Launcher Takes 6 turns" });
					goto setLandBase;

				case "Strategic Bomber":
					Deploy_ToolTip.Items.Add(new TextBlock() { Text = "Building Silo Takes 7 turns" });

				setLandBase:
					RegionOfDeployment.ItemsSource = new List<Nation>(Data.Me.SateliteNations) { Data.Me };
					break;

				case "Nuclear Submarine":
					Deploy_ToolTip.Items.Add(new TextBlock() { Text = "Building Silo Takes 12 turns" });

					RegionOfDeployment.ItemsSource = new List<Region>(Data.Regions.Where(r => r is Waters));
					break;

				default:
					return;
			}
		}

		private void Button_Deploy_Click(object sender, RoutedEventArgs e)
		{
			Platform prefab;

			switch ((NewPlatform.SelectedItem as ComboBoxItem)?.Content.ToString())
			{
				case "Silo":
					prefab = new Silo(Data.Me);
					break;

				case "Strategic Bomber":
					prefab = new StrategicBomber();
					break;

				case "Missile Launcher":
					prefab = new MissileLauncher();
					break;

				case "Nuclear Submarine":
					prefab = new NuclearSubmarine();
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

		private void NukeList_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			NukeList.SelectedItem = null;
		}
	}
}
