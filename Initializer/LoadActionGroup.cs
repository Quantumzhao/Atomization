#define FROM_XML

using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UIEngine;

namespace LCGuidebook.Initializer.Manager
{
	public static partial class Loader
	{
		public static ActionGroup BuildActionGroup(XmlNode node)
		{
			var name = node.Attributes["name"].Value;
			var description = node.Attributes["decription"]?.Value;
			var path = GeneratePath("library", "action_group", node.Attributes["src"].Value);
			var srcCode = ToCSCode(path);
			ActionGroup group = new ActionGroup(name, description);

			group.Script = CSharpScript.Create(srcCode,
				ScriptOptions.Default.WithReferences(typeof(Superpower).Assembly), typeof(Global));

			foreach (XmlNode cmdComplex in node.ChildNodes)
			{
				switch (cmdComplex.Name)
				{
					case "ActionGroup":
						group.AddSubGroup(BuildActionGroup(cmdComplex));
						break;

					case "Execution":
						group.AddAction(BuildExecution(cmdComplex, group));
						break;

					case "Field":
						group.AddAction(BuildField(cmdComplex, group));
						break;

					default:
						throw new InvalidOperationException($"\"{cmdComplex.Name}\" is bot a valid xml name");
				}
			}

			return group;
		}

		private static Execution BuildExecution(XmlNode node, ActionGroup parent)
		{
			var name = node.Attributes["name"].Value;
			var description = node.Attributes["description"]?.Value;
			var method = node.Attributes["method"].Value;
			Execution execution = new Execution(parent, name, method, description);

			return execution;
		}

		private static Field BuildField(XmlNode node, ActionGroup parent)
		{
			var name = node.Attributes["name"].Value;
			var bindedFieldName = node.Attributes["propertyName"]?.Value ?? name;
			var isReadOnly = bool.Parse(node.Attributes["isReadOnly"].Value);
			Field field = new Field(parent, name, bindedFieldName);
			if (isReadOnly)
			{
				field.AppendVisibleAttribute(new VisibleAttribute(name) { IsControlEnabled = false });
			}

			return field;
		}

		public static List<ActionGroup> GetAllActionGroups()
		{
			var TopLevelGroups = new List<ActionGroup>();

#if !FROM_XML
			foreach (var path in Directory.GetFiles(GeneratePath("interfaces", "action_group")))
			{
				foreach (XmlNode node in ToXmlDoc(path))
				{
					TopLevelGroups.Add(BuildActionGroup(node));
				}
			}
#else
			TopLevelGroups.Add(AssemblyCodeLoader.BuildActionGroup());
#endif
			return TopLevelGroups;
		}
	}
}
