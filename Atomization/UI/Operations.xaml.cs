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
	public partial class Operations : Page
	{
		public DeployNuke_Window DeployNuke_Window = null;

		public Operations()
		{
			InitializeComponent();
			foreach (var platform in Data.Me.NuclearPlatforms)
			{
				platform.NuclearWeapons.OnAddItem += (list, weapon) => NukeList.Items.Add(weapon);
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
	}
}
