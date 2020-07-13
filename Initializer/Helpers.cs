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

		private static MainIndexType ToFigure(string literal)
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