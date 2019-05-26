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
			Economy.Content = (int)Data.Me.Economy.Value.Value;
			Data.Me.Economy.Value.OnValueChanged += (n, pv, nv) => Economy.Content = (int)nv;
			#endregion

			#region High Education Population Initialization
			HiEduPopu.Content = (int)Data.Me.HiEduPopu.Value.Value;
			Data.Me.HiEduPopu.Value.OnValueChanged += (n, pv, nv) => HiEduPopu.Content = (int)nv;

			#endregion

			#region Army Initialization
			Army.Content = (int)Data.Me.Army.Value.Value;
			Data.Me.Army.Value.OnValueChanged += (n, pv, nv) => Army.Content = (int)nv;

			#endregion

			#region Navy Initialization
			Navy.Content = (int)Data.Me.Navy.Value.Value;
			Data.Me.Navy.Value.OnValueChanged += (n, pv, nv) => Navy.Content = (int)nv;

			#endregion

			#region Food Initialization
			Food.Content = (int)Data.Me.Food.Value.Value;
			Data.Me.Food.Value.OnValueChanged += (n, pv, nv) => Food.Content = (int)nv;

			#endregion

			#region Raw Material Initialization
			RawMaterial.Content = (int)Data.Me.Economy.Value.Value;
			Data.Me.RawMaterial.Value.OnValueChanged += (n, pv, nv) => RawMaterial.Content = (int)nv;

			#endregion

			#region Nuclear Initialization
			NuclearMaterial.Content = (int)Data.Me.NuclearMaterial.Value.Value;
			Data.Me.NuclearMaterial.Value.OnValueChanged += (n, pv, nv) => NuclearMaterial.Content = (int)nv;

			#endregion

			#region Stability Initialization
			ProgBar_Stability.Value = Data.Me.Stability.Value.Value;
			Stability.Content = (int)Data.Me.Stability.Value.Value;
			Data.Me.Stability.Value.OnValueChanged += (n, pv, nv) =>
			{
				Stability.Content = (int)nv;
				ProgBar_Stability.Value = nv;
			};

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
	}
}
