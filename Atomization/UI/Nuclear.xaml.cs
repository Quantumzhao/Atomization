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

			//foreach (var platform in Data.Me.NuclearPlatforms)
			//{
			//	platform.NuclearWeapons.OnItemAdded += (list, weapon) => NukeList.Items.Add(weapon);
			//	platform.NuclearWeapons.OnItemRemoved += (list, weapon) => NukeList.Items.Remove(weapon);
			//}
			NukeList.ItemsSource = Data.myNuclearWeapons;
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
			RegionOfDeployment.Items.Clear();
			switch ((NewPlatform.SelectedItem as ComboBoxItem)?.Content.ToString())
			{
				case "Silo":
				case "Missile Launcher":
				case "Strategic Bomber":
					RegionOfDeployment.Items.Add(
						new ComboBoxItem()
						{
							Content = Data.Me.Name,
							Padding = new Thickness(3)
						}
					);
					foreach (var item in Data.Me.SateliteNations)
					{
						RegionOfDeployment.Items.Add(
							new ComboBoxItem()
							{
								Content = item.Name,
								Padding = new Thickness(3)
							}
						);
					}
					break;

				case "Nuclear Submarine":
					RegionOfDeployment.Items.Add(
						new ComboBoxItem()
						{
							Content = Data.Me.TerritorialWaters.Name,
							Padding = new Thickness(3)
						}
					);
					foreach (var item in Data.Me.SateliteNations)
					{
						RegionOfDeployment.Items.Add(
							new ComboBoxItem()
							{
								Content = item.TerritorialWaters.Name,
								Padding = new Thickness(3)
							}
						);
					}

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
					prefab = new Silo();
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
			var name = (RegionOfDeployment.SelectedItem as ComboBoxItem).Content.ToString();
			prefab.DeployRegion = Data.Regions.Single(r => r.Name == name);
			Data.Me.NuclearPlatforms.Add(prefab);

			MessageBox.Show(
				"New nuclear strike platform deployed successfully", 
				"Intelligence", 
				MessageBoxButton.OK, 
				MessageBoxImage.Information);
		}
	}
}
