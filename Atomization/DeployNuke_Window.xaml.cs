﻿using System;
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			NuclearWeapon prefab;
			switch (((ComboBoxItem)CarrierType.SelectedItem)?.Content.ToString())
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

				default:
					return;
			}

			Warhead warhead;
			switch (((ComboBoxItem)WarheadType.SelectedItem)?.Content.ToString())
			{
				case "Hydrogen Bomb":
					warhead = new Hydrogen();
					break;

				case "Atomic Bomb Gen 1":
					warhead = new Atomic();
					break;

				case "Atomic Bomb Gen 2":
					warhead = new Atomic();
					break;

				case "Dirty Bomb":
					warhead = new Dirty();
					break;

				default:
					return;
			}

			prefab.Warheads.Add(warhead);

			prefab.Name = TextBox_Name.Text;

			prefab.Platform = SelectionList.SelectedItem as Platform;
			if (prefab.Platform == null) return;

			prefab.Platform.NuclearWeapons.Add(prefab);
			
			this.Close();
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
