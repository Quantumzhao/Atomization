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
		private const string _BAD_INIT_SEQ = "There's a dismatch between declared init order and enumeration order";
		public static void InitializeMe()
		{
			ResourceManager.Regions.Add(ResourceManager.Me = new Superpower() {Name = "C" });
			InitializeValues(ToXmlDoc(Resources.initial_values_initcfg));
			InitializeGrowth(ToXmlDoc(Resources.initial_growth_initcfg));
			AdditionalInitializations(ToXmlDoc(Resources.additional_initializations_initcfg));

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
					var code = ToCSCode(Resources.additional_initializations_csx);
					var script = CSharpScript.Create($"{code}{procedureName}()", ScriptOptions.Default.WithReferences(typeof(Superpower).Assembly));
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

				static Expression BuildExpression(XmlNode node)
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
						throw new FormatException(_BAD_LAMBDA_EXCEPTION);
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
		private static XmlNodeList ToXmlDoc(byte[] bytes)
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(stream);
				return doc.DocumentElement.ChildNodes;
			}
		}

		//private static string ToCSCode(string path) => File.ReadAllText(path);
		/// <summary>
		///		ignores the <c>#r</c> directives
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private static string ToCSCode(byte[] bytes)
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (StreamReader reader = new StreamReader(stream))
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
					return builder.ToString();
				}
			}
		}

		private static MainIndexType ToMainIndexType(string literal)
		{
			return (MainIndexType)Enum.Parse(typeof(MainIndexType), literal);
		}
	}
}
