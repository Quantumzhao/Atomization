using System;
using System.Windows;
using System.Windows.Controls;

namespace Atomization
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			#region Menu item initializations
			current = MainMenu.Items[0] as MenuItem;
			#endregion

			Data.Initiaze();

			DataContext = this;

			#region Economy initialization
			Economy.DataContext = Data.Me.Economy;
			EconomyGrowth.ItemsSource = Data.Me.Economy.Growth.Items;
			EconomyGrowth_Sum.DataContext = Data.Me.Economy.Growth;
			#endregion

			#region High Education Population Initialization
			HiEduPopu.DataContext = Data.Me.Population;
			HiEduGrowth.ItemsSource = Data.Me.Population.Growth.Items;
			HiEduGrowth_Sum.DataContext = Data.Me.Population.Growth;
			#endregion

			#region Army Initialization
			Army.DataContext = Data.Me.Army;
			ArmyGrowth.ItemsSource = Data.Me.Army.Growth.Items;
			ArmyGrowth_Sum.DataContext = Data.Me.Army.Growth;
			#endregion

			#region Navy Initialization
			Navy.DataContext = Data.Me.Navy;
			NavyGrowth.ItemsSource = Data.Me.Navy.Growth.Items;
			NavyGrowth_Sum.DataContext = Data.Me.Navy.Growth;
			#endregion

			#region Food Initialization
			Food.DataContext = Data.Me.Food;
			FoodGrowth.ItemsSource = Data.Me.Food.Growth.Items;
			FoodGrowth_SUm.DataContext = Data.Me.Food.Growth;
			#endregion

			#region Raw Material Initialization
			RawMaterial.DataContext = Data.Me.RawMaterial;
			RawMaterialGrowth.ItemsSource = Data.Me.RawMaterial.Growth.Items;
			RawMaterialGrowth_Sum.DataContext = Data.Me.RawMaterial.Growth;
			#endregion

			#region Nuclear Material Initialization
			NuclearMaterial.DataContext = Data.Me.NuclearMaterial;
			NuclearMaterialGrowth.ItemsSource = Data.Me.NuclearMaterial.Growth.Items;
			NuclearMaterialGrowth_Sum.DataContext = Data.Me.NuclearMaterial.Growth;
			#endregion

			#region Stability Initialization
			ProgBar_Stability.DataContext = Data.Me.Stability;
			Stability.DataContext = Data.Me.Stability;
			StabilityGrowth.ItemsSource = Data.Me.Stability.Growth.Items;
			StabilityGrowth_Sum.DataContext = Data.Me.Stability.Growth;
			#endregion
		}

		#region Menu item actions
		private MenuItem current;
		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			MenuItem selectedItem = sender as MenuItem;
			MainFrame.Source = new Uri(string.Format($"UI/{selectedItem.Header}.xaml"), UriKind.Relative);
			current.FontFamily = Properties.Settings.Default.Font_Semilight;
			selectedItem.FontFamily = Properties.Settings.Default.Font_Bold;
			current = selectedItem;
		}

		#endregion

		private void Button_MoreInfo_Click(object sender, RoutedEventArgs e)
		{
			var infoPanel = new MoreInfoPanel();
			infoPanel.Show();
		}

		private void newTurn()
		{
			Continue.IsEnabled = true;

			updateStatusBar();
		}
		private void updateStatusBar()
		{

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

		}

		private void Continue_Click(object sender, RoutedEventArgs e)
		{
		}
	}
}
