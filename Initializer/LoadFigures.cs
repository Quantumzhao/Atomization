using LCGuidebook.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		private static List<(string, string)> _FigureNamesMap = new List<(string, string)>();

		public static void InitializeFigures()
		{
			LoadFigureNamesMap(ToXmlDoc(GeneratePath("config", "value_definitions", "figures_definition.initcfg")));
		}

		private static void LoadFigureNamesMap(XmlNodeList doc)
		{
			foreach (XmlNode node in doc)
			{
				var name = node.Attributes["name"].InnerText;
				// identifier can be left blank, in which case is the same as name
				var id = node.Attributes["identifier"]?.InnerText ?? name;
				_FigureNamesMap.Add((id, name));
			}

			ResourceManager.NumOfFigures = _FigureNamesMap.Count;
		}

		public static string GetFigureNameById(string id) => _FigureNamesMap.Single(t => t.Item1 == id).Item2;
		public static int GetIndexById(string id)
		{
			for (int i = 0; i < _FigureNamesMap.Count; i++)
			{
				if (_FigureNamesMap[i].Item1 == id) return i;
			}

			throw new ArgumentException();
		}
	}
}