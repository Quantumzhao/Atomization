using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using LCGuidebook.Core.DataStructures;
using LCGuidebook.Core;
using Initializer.Properties;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		private const string _BAD_REGION_TYPE = "No such type of region";
		private static void InitializeMap(XmlNodeList doc)
		{
			if (doc.Count != 2)
			{
				throw new InvalidOperationException(_BAD_GRAPH);
			}

			XmlNodeList declaration = null;
			XmlNodeList relation = null;
			foreach (XmlNode section in doc)
			{
				switch (section.Name)
				{
					case "declaration":
						declaration = section.ChildNodes;
						break;

					case "relation":
						relation = section.ChildNodes;
						break;

					default:
						throw new InvalidOperationException();
				}
			}

			var graph = BuildEdges(BuildVertices(declaration), relation);
		}

		private static List<(string, Region)> BuildVertices(XmlNodeList doc)
		{
			var regions = new List<(string, Region)>();

			foreach (XmlNode vertex in doc)
			{
				var name = vertex.Attributes["name"].InnerText;
				var i = vertex.Attributes["identifier"];
				var identifier = i switch
				{
					null => name,
					_ => i.InnerText
				};
				var type = vertex.Attributes["type"];

				switch (type.InnerText)
				{
					case "superpower":
						regions.Add((identifier, new Superpower() { Name = name }));
						break;

					case "nation":
						regions.Add((identifier, new RegularNation() { Name = name }));
						break;

					case "water":
						regions.Add((identifier, new Waters() { Name = name }));
						break;

					default:
						throw new InvalidOperationException(_BAD_REGION_TYPE);
				}
			}

			return regions;
		}

		private static (List<Superpower>, List<Nation>, List<Waters>) BuildEdges
			(List<(string, Region)> list, XmlNodeList doc)
		{
			Region valueOf(string id) => list.Find(r => r.Item1 == id).Item2;

			var superpowers = new List<Superpower>();
			var nations = new List<Nation>();
			var waters = new List<Waters>();

			foreach (XmlNode edge in doc)
			{
				var from = valueOf(edge.Attributes["from"].InnerText);
				var to = valueOf(edge.Attributes["to"].InnerText);

				if (!from.Neighbors.Contains(to))
				{
					from.Neighbors.Add(to);
				}
				if (!to.Neighbors.Contains(from))
				{
					to.Neighbors.Add(from);
				}

				if (from is Superpower superpower)
				{
					superpowers.Add(superpower);
				}
				else if (from is RegularNation nation)
				{
					nations.Add(nation);
				}
				else
				{
					waters.Add(from as Waters);
				}
			}

			return (superpowers, nations, waters);
		}
	}
}