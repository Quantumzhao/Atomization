using System.Collections.Generic;
using System.Xml;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		private static Dictionary<string, string> _FigureNamesMap = new Dictionary<string, string>();

		public static void InitializeFigures()
		{
			LoadFigureNamesMap(ToXmlDoc(GeneratePath("config", "value_definitions", "figures_definition.initcfg")));
		}

		private static void LoadFigureNamesMap(XmlNodeList doc)
		{
			foreach (XmlNode node in doc)
			{
				var name = node.Attributes["name"].InnerText;
				var id = node.Attributes["identifier"]?.InnerText ?? name;
				_FigureNamesMap.Add(id, name);
			}
		}

	}
}