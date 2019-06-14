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
			Data.Me.Economy.OnValueChanged += (n, pv, nv) => Economy.Content = (int)nv;

			EconomyGrowth.ItemsSource = Data.Me.Economy.Growth.Values;
			
			EconomyGrowth_Sum.Text = Data.Me.Economy.Growth.Sum.ToString();
			Data.Me.Economy.Growth.CollectionChanged += (sender, e) =>
				EconomyGrowth_Sum.Text = Data.Me.Economy.Growth.Sum.ToString();
			#endregion

			#region High Education Population Initialization
			HiEduPopu.Content = (int)Data.Me.HiEduPopu.Value_Numerical;
			Data.Me.HiEduPopu.OnValueChanged += (n, pv, nv) => HiEduPopu.Content = (int)nv;

			HiEduGrowth.ItemsSource = Data.Me.HiEduPopu.Growth.Values;

			HiEduGrowth_Sum.Text = Data.Me.HiEduPopu.Growth.Sum.ToString();
			Data.Me.HiEduPopu.Growth.CollectionChanged += (sender, e) =>
				HiEduGrowth_Sum.Text = Data.Me.HiEduPopu.Growth.Sum.ToString();
			#endregion

			#region Army Initialization
			Army.Content = (int)Data.Me.Army.Value_Numerical;
			Data.Me.Army.OnValueChanged += (n, pv, nv) => Army.Content = (int)nv;

			ArmyGrowth.ItemsSource = Data.Me.Army.Growth.Values;

			ArmyGrowth_Sum.Text = Data.Me.Army.Growth.Sum.ToString();
			Data.Me.Army.Growth.CollectionChanged += (sender, e) =>
				ArmyGrowth_Sum.Text = Data.Me.Army.Growth.Sum.ToString();
			#endregion

			#region Navy Initialization
			Navy.Content = (int)Data.Me.Navy.Value_Numerical;
			Data.Me.Navy.OnValueChanged += (n, pv, nv) => Navy.Content = (int)nv;

			NavyGrowth.ItemsSource = Data.Me.Navy.Growth.Values;

			NavyGrowth_Sum.Text = Data.Me.Navy.Growth.Sum.ToString();
			Data.Me.Navy.Growth.CollectionChanged += (sender, e) =>
				NavyGrowth_Sum.Text = Data.Me.Navy.Growth.Sum.ToString();
			#endregion

			#region Food Initialization
			Food.Content = (int)Data.Me.Food.Value_Numerical;
			Data.Me.Food.OnValueChanged += (n, pv, nv) => Food.Content = (int)nv;

			FoodGrowth.ItemsSource = Data.Me.Food.Growth.Values;

			FoodGrowth_SUm.Text = Data.Me.Food.Growth.Sum.ToString();
			Data.Me.Food.Growth.CollectionChanged += (sender, e) =>
				FoodGrowth_SUm.Text = Data.Me.Food.Growth.Sum.ToString();

			#endregion

			#region Raw Material Initialization
			RawMaterial.Content = (int)Data.Me.Economy.Value_Numerical;
			Data.Me.RawMaterial.OnValueChanged += (n, pv, nv) => RawMaterial.Content = (int)nv;

			RawMaterialGrowth.ItemsSource = Data.Me.RawMaterial.Growth.Values;

			RawMaterialGrowth_Sum.Text = Data.Me.RawMaterial.Growth.Sum.ToString();
			Data.Me.RawMaterial.Growth.CollectionChanged += (sender, e) =>
				RawMaterialGrowth_Sum.Text = Data.Me.RawMaterial.Growth.Sum.ToString();
			#endregion

			#region Nuclear Initialization
			NuclearMaterial.Content = (int)Data.Me.NuclearMaterial.Value_Numerical;
			Data.Me.NuclearMaterial.OnValueChanged += (n, pv, nv) => NuclearMaterial.Content = (int)nv;

			NuclearMaterialGrowth.ItemsSource = Data.Me.NuclearMaterial.Growth.Values;

			NuclearMaterialGrowth_Sum.Text = Data.Me.NuclearMaterial.Growth.Sum.ToString();
			Data.Me.NuclearMaterial.Growth.CollectionChanged += (sender, e) =>
				NuclearMaterialGrowth_Sum.Text = Data.Me.NuclearMaterial.Growth.Sum.ToString();
			#endregion

			#region Stability Initialization
			ProgBar_Stability.Value = Data.Me.Stability.Value_Numerical;
			Stability.Content = (int)Data.Me.Stability.Value_Numerical;
			Data.Me.Stability.OnValueChanged += (n, pv, nv) =>
			{
				Stability.Content = (int)nv;
				ProgBar_Stability.Value = nv;
			};

			StabilityGrowth.ItemsSource = Data.Me.Stability.Growth.Values;

			StabilityGrowth_Sum.Text = Data.Me.Stability.Growth.Sum.ToString();
			Data.Me.Stability.Growth.CollectionChanged += (sender, e) =>
				StabilityGrowth_Sum.Text = Data.Me.Stability.Growth.Sum.ToString();
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

	}
}
