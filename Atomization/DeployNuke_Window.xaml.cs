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
using System.Windows.Shapes;

namespace Atomization
{
	/// <summary>
	/// Interaction logic for DeployNuke_Window.xaml
	/// </summary>
	public partial class DeployNuke_Window : Window
	{
		public Operations ParentPage;
		public DeployNuke_Window()
		{
			InitializeComponent();
			Closing += DeployNuke_Window_Closing;

			foreach (var item in Data.Me.NuclearPlatforms)
			{
				SelectionList.Items.Add(item);
			}
		}

		private void DeployNuke_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ParentPage.DeployNuke_Window = null;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			NumOfWarheads.IsEnabled = true;
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			NumOfWarheads.IsEnabled = false;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NuclearWeapon prefab;
			switch (CarrierType.SelectedItem.ToString())
			{
				case "Cruise Missile":
					prefab = new NuclearMissile();
					break;

				case "Medium Range Missile":
					prefab = new NuclearMissile();
					break;

				case "ICBM":
					prefab = new NuclearMissile();
					break;

				case "Aerial Bomb":
					prefab = new NuclearBomb();
					break;
			}
		}

		private void SelectionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (SelectionList.SelectedItem is StrategicBomber)
			{
				for (int i = 0; i < 3; i++)
				{
					var current = CarrierType.Items[i] as ComboBoxItem;
					current.IsEnabled = false;
				}
				(CarrierType.Items[3] as ComboBoxItem).IsEnabled = true;
			}
			else
			{
				(CarrierType.Items[3] as ComboBoxItem).IsEnabled = false;
				for (int i = 0; i < 3; i++)
				{
					var current = CarrierType.Items[i] as ComboBoxItem;
					current.IsEnabled = true;
				}
			}
		}
	}
}
