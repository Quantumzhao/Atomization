using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using LCGuidebook.Core.DataStructures;
using LCGuidebook.Core;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Initializer.Properties;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
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
						throw new InvalidOperationException($"\"{cmdComplex.Name}\" is bot a valid xml name");
				}
			}

			return group;
		}

		private static Command BuildCommand(XmlNode node)
		{
			var name = node.Attributes["name"].Value;
			var description = node.Attributes["description"]?.Value;
			var signature = new List<Command.Parameter>();
			Command command = new Command(name, description);
			Func<Delegate> buildBody = null;

			foreach (XmlNode part in node.ChildNodes)
			{
				switch (part.Name)
				{
					case "Body":
						buildBody = () => BuildBody(part, command.Signature);
						break;

					case "Parameter":
						signature.Add(BuildParameter(part));
						break;

					default:
						break;
				}
			}
			command.Signature = signature.ToArray();
			command.Body = buildBody();

			return command;
		}

		public static List<CommandGroup> GetAllCommandGroups()
		{
			var TopLevelGroups = new List<CommandGroup>();

			foreach (var path in Directory.GetFiles(GeneratePath("interfaces", "commands")))
			{
				foreach (XmlNode node in ToXmlDoc(path))
				{
					TopLevelGroups.Add(BuildCommandGroup(node));
				}
			}
			return TopLevelGroups;
		}

		private static Script BuildScript(string path, string title, Command.Parameter[] parameters)
		{
			var srcCode = ToCSCode(path);
			var paramsLiteral = new StringBuilder();
			for (int i = 0; i < parameters.Length; i++)
			{
				// code gen part
				// result is like "(LCGuidebook.Core.Datastructure.Platform.Type)lcgGlobalVar[0], ..."
				paramsLiteral.Append($"({parameters[i].ObjectType.ToString().Replace('+', '.')}){nameof(Global.LcgGlobalVars)}[{i}]");
				if (i != parameters.Length - 1)
				{
					paramsLiteral.Append(", ");
				}
			}

			Script script = CSharpScript.Create($"{srcCode}\n{title}({paramsLiteral})", 
				ScriptOptions.Default.WithReferences(typeof(Superpower).Assembly), typeof(Global));

			return script;
		}

		private static Command.Parameter BuildParameter(XmlNode node)
		{
			var name = node.Attributes["displayName"].Value;
			var type = node.Attributes["type"].Value;
			var description = node.Attributes["description"]?.Value;

			return new Command.Parameter(type, name, description);
		}

		private	static Delegate BuildBody(XmlNode node, Command.Parameter[] parameters)
		{
			var src = GeneratePath("library", "commands", node.Attributes["src"].Value);
			var title = node.Attributes["title"].Value;

			var script = BuildScript(src, title, parameters);

			Action<object[]> callBack = args => script.RunAsync(new Global(args));
			return callBack;
		}
	}
}
