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
using System.ComponentModel;
using System.Collections.Specialized;

namespace Atomization
{
	/// <summary>
	/// Interaction logic for MoreInfoPanel.xaml
	/// </summary>
	public partial class MoreInfoPanel : Window
	{
		public MoreInfoPanel()
		{
			InitializeComponent();
		}
	}

	public class Sentence : INotifyCollectionChanged, INotifyPropertyChanged
	{
		private Dictionary<string, object> texts = new Dictionary<string, object>();

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		public void Add(string name, object value)
		{
			texts.Add(name, value);
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (var item in texts)
			{
				builder.Append(item.ToString());
			}
			return builder.ToString();
		}
	}
}
