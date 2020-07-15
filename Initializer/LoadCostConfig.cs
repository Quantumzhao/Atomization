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
				if (filePath.EndsWith("Task.Census.costcfg"))
				{
					foreach (XmlNode node in doc)
					{
						var id = node.Attributes["id"].InnerText;
						var (_, censusCost) = BuildCost(node.FirstChild);
						ResourceManager.RegisterCost($"LCG.{GetFigureNameById(id)}", TypesOfCostOfStage.Census, censusCost);
					}
				}
				else
				{
					foreach (XmlNode cost in doc[0].ChildNodes)
					{
						var tup = BuildCost(cost);
						ResourceManager.RegisterCost(doc[0].Attributes["name"].InnerText.Trim(), 
							tup.Item1, tup.Item2);
					}
				}
			}
		}

		private static (TypesOfCostOfStage, CostOfStage) BuildCost(XmlNode node)
		{
			TypesOfCostOfStage type;
			if (node.Attributes?["title"] == null)
			{
				type = TypesOfCostOfStage.Census;
			}
			else
			{
				type = (TypesOfCostOfStage)
				Enum.Parse(typeof(TypesOfCostOfStage), node.Attributes["title"].InnerText.Trim());
			}
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
						requiredTime = BuildExpression(child.FirstChild);
						break;

					default:
						break;
				}
			}
			return (type, new CostOfStage(longTermEffect, shortTermEffect, requiredTime));
		}

		private static Effect BuildEffect(XmlNode node)
		{
			Effect effect = Effect.GenerateEmptyEffect();
			foreach (XmlNode target in node.FirstChild.ChildNodes)
			{
				int index = GetIndexById(target.Attributes["id"].InnerText);
				var expression = BuildExpression(target.FirstChild);
				effect[index] = expression;
			}
			return effect;
		}
	}
}
