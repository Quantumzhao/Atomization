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
	/// Interaction logic for MoreInfoPanel.xaml
	/// </summary>
	public partial class MoreInfoPanel : Window
	{
		public MoreInfoPanel()
		{
			InitializeComponent();

			Renderer = new Renderer(InfoPanel);
		}

		public Renderer Renderer { get; private set; }
	}

	public class Renderer
	{
		public Renderer(StackPanel infoPanel)
		{

		}

		private List<IElement> elements = new List<IElement>();

		public void AddTitle()
		{

		}
		public void AddSubtitle()
		{

		}
		public void AddText()
		{

		}

		public void Update()
		{

		}

		private interface IElement
		{
			string Name { get; set; }
			string Content { get; set; }
			string Caption { get; set; }
		}

		private class Title : IElement
		{
			public string Name { get; set; }
			public string Content { get; set; }
			public string Caption { get; set; }
		}

		private class Subtitle : IElement
		{
			public string Name { get; set; }
			public string Content { get; set; }
			public string Caption { get; set; }
		}

		private class Text : IElement
		{
			public string Name { get; set; }
			public string Content { get; set; }
			public string Caption { get; set; }
			public string Value { get; set; }
		}
	}
}
