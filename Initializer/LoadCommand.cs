using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using LCGuidebook.Core.DataStructures;
using LCGuidebook.Core;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace LCGuidebook.Initialization.Manager
{
	public static partial class Loader
	{
		private static XmlNodeList LoadXmlRootElements(string url)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(url);
			return doc.DocumentElement.ChildNodes;
		}

		public static CommandGroup BuildCommandGroup(XmlNode node)
		{
			var name = node.Attributes["name"].Value;
			var description = node.Attributes["decription"]?.Value;
			CommandGroup group = new CommandGroup(name, description);

			foreach (XmlNode cmdComplex in node.ChildNodes)
			{
				switch (cmdComplex.Name)
				{
					case "CommandGroup":
						group.AddSubGroup(BuildCommandGroup(cmdComplex));
						break;

					case "Command":
						group.AddCommand(BuildCommand(cmdComplex));
						break;

					default:
						throw new InvalidOperationException($"Not a valid name. Name is {cmdComplex.Name}");
				}
			}

			return group;
		}

		public static Command BuildCommand(XmlNode node)
		{
			var name = node.Attributes["name"].Value;
			var description = node.Attributes["description"]?.Value;
			Command command = new Command(name, description);

			foreach (XmlNode part in node.ChildNodes)
			{
				switch (part.Name)
				{
					case "Body":
						command.Body = BuildBody(part);
						break;

					case "Parameter":
						command.Signature.Add(BuildParameter(part));
						break;

					default:
						break;
				}
			}

			throw new NotImplementedException();
		}

		public static List<CommandGroup> GetAllCommandGroups()
		{
			List<CommandGroup> TopLevelGroups = new List<CommandGroup>();

			//string currentDir = Directory.GetCurrentDirectory();

			foreach (XmlNode node in LoadXmlRootElements($"{ResourceManager.Misc.SolutionPath}\\Initializer\\interfaces\\test.csxh"))
			{
				TopLevelGroups.Add(BuildCommandGroup(node));
			}

			return TopLevelGroups;
		}

		private static Script<object> BuildScript(string src, string title, params Command.Parameter[] parameters)
		{			
			//Script<object> script = CSharpScript.Create($"#load {_DEFINITION_DIRECTORY}\\{src}  ",);

			throw new NotImplementedException();
		}

		private static Command.Parameter BuildParameter(XmlNode node)
		{
			var name = node.Attributes["displayName"].Value;
			var type = node.Attributes["type"].Value;
			var description = node.Attributes["description"]?.Value;

			return new Command.Parameter(type, name, description);
		}

		private	static Delegate BuildBody(XmlNode node)
		{
			var src = node.Attributes["src"].Value;
			var title = node.Attributes["title"].Value;

			throw new NotImplementedException();
		}
	}
}
