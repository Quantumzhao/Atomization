using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using LCGuidebook.Core.DataStructures;
using LCGuidebook.Core;

namespace LCGuidebook.Initialization.Manager
{
	public static partial class Loader
	{
		public static void InitializeMe()
		{
			var doc = LoadXmlRootElements($"{ResourceManager.Misc.SolutionPath}\\Initializer\\config\\initial_values.initcfg");

			foreach (XmlNode node in doc)
			{
				if (node.NodeType != XmlNodeType.Comment)
				{
					var index = int.Parse(node.Attributes["index"].Value);
					var value = int.Parse(node.InnerText);
					ResourceManager.Me.NationalIndices[index].CurrentValue = value;
				}
			}
		}
	}
}
