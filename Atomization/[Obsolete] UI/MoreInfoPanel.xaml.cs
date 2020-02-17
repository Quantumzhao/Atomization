using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Core
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
