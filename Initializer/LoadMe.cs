using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using LCGuidebook.Core.DataStructures;
using LCGuidebook.Core;
using System.Text.RegularExpressions;
using Initializer.Properties;
using System.Dynamic;
using System.Reflection.PortableExecutable;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		private const string _BAD_LAMBDA_EXCEPTION = "Invalid lambda expression";
		private const string _BAD_INIT_SEQ = 
			"There's a disparity between declared init order and enumeration order";
		private const string _BAD_GRAPH = "There must be exactly 1 declaration section and 1 relation section ";

		public static void InitializeMe()
		{
			//ResourceManager.Regions.Add(ResourceManager.Me = new Superpower() {Name = "C" });
			SetMe();
			InitializeValues(ToXmlDoc(GeneratePath("config", "value_definitions", "initial_values.initcfg")));
			InitializeGrowth(ToXmlDoc(GeneratePath("config", "value_definitions", "initial_growth.initcfg")));
			AdditionalInitializations(ToXmlDoc(GeneratePath(
				"interfaces", "setup", "additional_initializations.initact")));
		}

		private static void InitializeValues(XmlNodeList doc)
		{
			foreach (XmlNode node in doc)
			{
				if (node.NodeType != XmlNodeType.Comment)
				{
					var name = GetFigureNameById(node.Attributes["id"].Value);
					var value = int.Parse(node.InnerText);
					ResourceManager.Me.Figures.Add(new ValueComplex(name, value));
				}
			}
		}

		private static void AdditionalInitializations(XmlNodeList doc)
		{
			int counter = 0;
			foreach (XmlNode proc in doc)
			{
				if (proc.NodeType == XmlNodeType.Comment) continue;
				if (int.Parse(proc.Attributes["order"].Value) != counter)
				{
					throw new ArgumentException(_BAD_INIT_SEQ);
				}

				var procedureName = proc.InnerText.Trim();
				var code = ToCSCode(GeneratePath("library", "setup", proc.Attributes["file"].InnerText));
				var script = CSharpScript.Create($"{code}{procedureName}()",
					ScriptOptions.Default.WithReferences(typeof(Superpower).Assembly));
				script.RunAsync();
			}
		}

		private static void InitializeGrowth(XmlNodeList doc)
		{
			foreach (XmlNode growth in doc)
			{
				if (growth.NodeType == XmlNodeType.Comment) continue;

				var name = growth.Attributes["name"].Value;

				foreach (XmlNode target in growth.ChildNodes)
				{
					var figureName = GetFigureNameById(target.Attributes["id"].Value);
					var expression = BuildExpression(target.FirstChild);
					ResourceManager.Me.GetFigureByName(figureName).Growth.AddTerm(name, expression);
				}
			}
		}

		private static void SetMe()
		{
			// placeholder, will change in the future
			ResourceManager.Me = ResourceManager.Regions.Find(r => r.Name == "China") as Superpower;
		}
	}
}
