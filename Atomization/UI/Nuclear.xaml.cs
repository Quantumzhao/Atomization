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
			foreach (var platform in Data.Me.NuclearPlatforms)
			{
				platform.NuclearWeapons.OnAddItem += (list, weapon) => NukeList.Items.Add(weapon);
				platform.NuclearWeapons.OnRemoveItem += (list, weapon) => NukeList.Items.Remove(weapon);
			}
		}
		private void Button_DeployNuke_Click(object sender, RoutedEventArgs e)
		{
			if (DeployNuke_Window == null)
			{
				DeployNuke_Window = new DeployNuke_Window() { ParentPage = this };
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
	}
}
