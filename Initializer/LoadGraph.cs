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
using System.Linq;
using System.Runtime.InteropServices;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		private const string _BAD_REGION_TYPE = "No such type of region";
		private const string _BAD_RELATION = "Only \"edge\" and \"inclination\" is valid";
		private const string _BAD_SECTION = "Only \"declaration\" and \"relation\" is valid";
		private const string _BAD_INCL_DESC = "Either \"from\" or \"to\" is not a nation";
		private const string _BAD_SVRN_DESC = "Either \"from\" or \"to\" is not a valid type";

		public static void InitializeMap()
		{
			InitializeMap(ToXmlDoc(GeneratePath("config", "map", "GameBoard.lcgmap")));
		}
		private static void InitializeMap(XmlNodeList doc)
		{
			if (doc.Count != 2)
			{
				throw new InvalidDataException(_BAD_GRAPH);
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
						throw new InvalidDataException(_BAD_SECTION);
				}
			}

			ResourceManager.Regions = BuildRelations(BuildVertices(declaration), relation);
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
						regions.Add((identifier, new Superpower(name)));
						break;

					case "nation":
						regions.Add((identifier, new RegularNation(name)));
						break;

					case "water":
						regions.Add((identifier, new Waters(name)));
						break;

					default:
						throw new InvalidDataException(_BAD_REGION_TYPE);
				}
			}

			return regions;
		}

		private static List<Region> BuildRelations(List<(string, Region)> list, XmlNodeList doc)
		{
			Region valueOf(string id) => list.Find(r => r.Item1 == id).Item2;


			foreach (XmlNode node in doc)
			{
				var from = valueOf(node.Attributes["from"].InnerText);
				var to = valueOf(node.Attributes["to"].InnerText);
				var w = node.Attributes["weight"];
				var weight = w switch { null => 0, _ => double.Parse(w.InnerText) };

				switch (node.Name)
				{
					case "edge":
						// assume weight is 1 for now
						BuildEdge(from, to, 1);
						break;

					case "inclination":
						if (from is Nation nFrom && to is Nation nTo)
						{
							BuildInclination(nFrom, nTo, weight);
						}
						else
						{
							throw new InvalidDataException(_BAD_INCL_DESC);
						}
						break;

					case "sovereign":
						if (from is Nation obj && to is Waters sbj)
						{
							BuildSovereign(obj, sbj);
						}
						else
						{
							throw new InvalidDataException(_BAD_SVRN_DESC);
						}
						break;

					default:
						throw new InvalidDataException(_BAD_RELATION);
				}
			}

			return list.Select(t => t.Item2).ToList();
		}

		private static void BuildEdge(Region from, Region to, double weight)
		{
			if (!from.Neighbors.Contains(to))
			{
				from.Neighbors.Add(to);
			}
			if (!to.Neighbors.Contains(from))
			{
				to.Neighbors.Add(from);
			}
		}

		private static void BuildInclination(Nation from, Nation to, double weight)
		{
			from.Inclination.Add(to, weight);
		}

		private static void BuildSovereign(Nation from, Waters to)
		{
			from.TerritorialSea.Add(to);
			to.Sovereign = from;
		}
	}
}