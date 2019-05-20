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
