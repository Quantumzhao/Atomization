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

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		private const string _BAD_LAMBDA_EXCEPTION = "Invalid lambda expression";
		private const string _BAD_INIT_SEQ = 
			"There's a dismatch between declared init order and enumeration order";
		public static void InitializeMe()
		{
			ResourceManager.Regions.Add(ResourceManager.Me = new Superpower() {Name = "C" });
			InitializeValues(ToXmlDoc(GeneratePath("config", "initial_values.initcfg")));
			InitializeGrowth(ToXmlDoc(GeneratePath("config", "initial_growth.initcfg")));
			AdditionalInitializations(ToXmlDoc(GeneratePath(
				"interfaces", "setup", "additional_initializations.initact")));

			static void InitializeValues(XmlNodeList doc)
			{
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
			static void AdditionalInitializations(XmlNodeList doc)
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
					var code = ToCSCode(GeneratePath("library", "setup", "additional_initializations.csx"));
					var script = CSharpScript.Create($"{code}{procedureName}()", 
						ScriptOptions.Default.WithReferences(typeof(Superpower).Assembly));
					script.RunAsync();
				}
			}
			static void InitializeGrowth(XmlNodeList doc)
			{
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
		}

		private static XmlNodeList ToXmlDoc(string path)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			return doc.DocumentElement.ChildNodes;
		}

		/// <summary>
		///		ignores the <c>#r</c> directives
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private static string ToCSCode(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				var builder = new StringBuilder();
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					if (!line.StartsWith("#r"))
					{
						builder.Append(line);
					}
				}
				builder.Append("\n");

				return builder.ToString();
			}
		}

		private static MainIndexType ToMainIndexType(string literal)
		{
			return (MainIndexType)Enum.Parse(typeof(MainIndexType), literal);
		}

		private static string GeneratePath(params string[] relativePath)
		{
			StringBuilder stringBuilder = new StringBuilder(Resources.InitializerRootPath);
			for (int i = 0; i < relativePath.Length; i++)
			{
				stringBuilder.Append($"/{relativePath[i]}");
			}
			return stringBuilder.ToString();
		}

		private static Expression BuildExpression(XmlNode node)
		{
			double constant = double.Parse(node.Attributes["coefficient"].InnerText.Trim());
			var expBodyLit = node.InnerText;

			if (expBodyLit.Trim() == string.Empty)
			{
				return new Expression(constant);
			}

			if (Regex.IsMatch(expBodyLit, "=>"))
			{
				var results = Regex.Matches(expBodyLit, @"[A-Z][a-z]+");
				var expBodyLitCpy = 
					$"using LCGuidebook.Core; using LCGuidebook.Core.DataStructures; {expBodyLit.Trim()}";
				foreach (Match result in results)
				{
					expBodyLitCpy = expBodyLitCpy.Replace(result.Value, 
						$"ResourceManager.Me.NationalIndices[MainIndexType.{result.Value}].CurrentValue");
				}
				var option = ScriptOptions.Default.AddReferences(typeof(ValueComplex).Assembly);
				Func<double, double> expBody = CSharpScript.EvaluateAsync<Func<double, double>>
					(expBodyLitCpy, option).Result;
				return new Expression(constant, expBody);
			}
			else
			{
				throw new FormatException(_BAD_LAMBDA_EXCEPTION);
			}
		}
	}
}
