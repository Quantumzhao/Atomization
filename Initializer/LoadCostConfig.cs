using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LCGuidebook.Core.DataStructures;
using System.Xml;
using LCGuidebook.Core;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		public static void InitializeCosts()
		{
			var filePaths = Directory.GetFiles(GeneratePath("config", "costs"));
			foreach (var filePath in filePaths)
			{
				var doc = ToXmlDoc(filePath);
				foreach (XmlNode cost in doc[0].ChildNodes)
				{
					var tup = BuildCost(cost);
					ResourceManager.RegisterCost(doc[0].Attributes["name"].InnerText.Trim(), 
						tup.Item1, tup.Item2);
				}
			}
		}

		private static (TypesOfCostOfStage, CostOfStage) BuildCost(XmlNode node)
		{
			var type = (TypesOfCostOfStage)
				Enum.Parse(typeof(TypesOfCostOfStage), node.Attributes["title"].InnerText.Trim());
			Effect longTermEffect = null;
			Effect shortTermEffect = null;
			Expression requiredTime = null;
			foreach (XmlNode child in node.ChildNodes)
			{
				switch (child.Name)
				{
					case "effect":
						var effect = BuildEffect(child);
						if (child.Attributes["duration"].InnerText.Trim() == "long")
						{
							longTermEffect = effect;
						}
						else
						{
							shortTermEffect = effect;
						}
						break;

					case "requiredTime":
						requiredTime = BuildExpression(child);
						break;

					default:
						break;
				}
			}
			return (type, new CostOfStage(longTermEffect, shortTermEffect, requiredTime));
		}

		private static Effect BuildEffect(XmlNode node)
		{
			Effect effect = new Effect();
			foreach (XmlNode target in node.ChildNodes)
			{
				MainIndexType type = ToMainIndexType(target.Attributes["lcg:mainIndexTitle"]
					.InnerText.Trim());
				var expression = BuildExpression(target.FirstChild);
				effect[type] = expression;
			}
			return effect;
		}
	}
}
