using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using LCGuidebook.Core.DataStructures;
using LCGuidebook.Core;
using System.Text.RegularExpressions;

namespace LCGuidebook.Initialization.Manager
{
	public static partial class Loader
	{
		private const string _BAD_LANBDA_EXCEPTION = "Invalid lambda expression";
		public static void InitializeMe()
		{
			ResourceManager.Regions.Add(ResourceManager.Me = new Superpower() {Name = "C" });
			InitializeValues("initial_values.initcfg");
			InitializeGrowth("initial_growth.initcfg");
			AdditionalInitializations();
		}

		private static void InitializeValues(string fileName)
		{
			var doc = LoadXmlRootElements($"{ResourceManager.Misc.SolutionPath}/Initializer/config/{fileName}");

			foreach (XmlNode node in doc)
			{
				if (node.NodeType != XmlNodeType.Comment)
				{
					var index = ToMainIndexType(node.Attributes["lcg:mainIndexTitle"].Value);
					var value = int.Parse(node.InnerText);
					ResourceManager.Me.NationalIndices[index].CurrentValue = value;
				}
			}
		}

		private static void InitializeGrowth(string fileName)
		{
			var doc = LoadXmlRootElements($"{ResourceManager.Misc.SolutionPath}/Initializer/config/{fileName}");

			foreach (XmlNode growth in doc)
			{
				if (growth.NodeType == XmlNodeType.Comment) continue;

				var name = growth.Attributes["name"].Value;

				foreach (XmlNode target in growth.ChildNodes)
				{
					var idx = ToMainIndexType(target.Attributes["lcg:mainIndexTitle"].Value);
					var expression = BuildExpression(target.FirstChild);
					ResourceManager.Me.NationalIndices[idx].Growth.AddTerm(name, expression);
				}				
			}
		}

		private static void AdditionalInitializations()
		{

		}
		private static Expression BuildExpression(XmlNode node)
		{
			double currentValue;
			var mainIndexTitle = node.Attributes["parameterIndex"];
			var expBodyLit = node.InnerText;

			if (mainIndexTitle != null)
			{
				var index = ToMainIndexType(mainIndexTitle.Value);
				currentValue = ResourceManager.Me.NationalIndices[index].CurrentValue;
			}
			else
			{
				return new Expression(int.Parse(expBodyLit));
			}

			if (Regex.IsMatch(expBodyLit, "=>"))
			{
				var option = ScriptOptions.Default.AddReferences(typeof(ValueComplex).Assembly);
				Func<double, double> expBody = CSharpScript.EvaluateAsync<Func<double, double>>(expBodyLit, option).Result;
				return new Expression(currentValue, expBody);
			}
			else
			{
				throw new FormatException(_BAD_LANBDA_EXCEPTION);
			}
		}
		private static MainIndexType ToMainIndexType(string literal)
		{
			return (MainIndexType)Enum.Parse(typeof(MainIndexType), literal);
		}
	}
}
