﻿using LCGuidebook.Core;
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
			var path = GeneratePath("library", "commands", node.Attributes["src"].Value);
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

					case "Bulletinboard":
						group.AddAction(BuildBulletinboard(cmdComplex, group));
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
			//var signature = new List<Execution.Parameter>();
			Execution execution = new Execution(parent, name, method, description);
			//Func<Delegate> buildBody = null;

			//foreach (XmlNode part in node.ChildNodes)
			//{
			//	switch (part.Name)
			//	{
			//		case "Body":
			//			buildBody = () => BuildBody(part, execution.Signature);
			//			break;

			//		case "Parameter":
			//			signature.Add(BuildParameter(part));
			//			break;

			//		default:
			//			break;
			//	}
			//}
			//execution.Signature = signature.ToArray();
			//execution.Body = buildBody();

			return execution;
		}

		private static Field BuildBulletinboard(XmlNode node, ActionGroup parent)
		{
			var name = node.Attributes["name"].Value;
			var isReadOnly = bool.Parse(node.Attributes["isReadOnly"].Value);
			Field bulletinboard = new Field(parent);
			if (isReadOnly)
			{
				// intended to mark it readonly, will do it later
				bulletinboard.AppendVisibleAttribute(new VisibleAttribute(name));
			}

			return bulletinboard;
		}

		public static List<ActionGroup> GetAllActionGroups()
		{
			var TopLevelGroups = new List<ActionGroup>();

			foreach (var path in Directory.GetFiles(GeneratePath("interfaces", "action_group")))
			{
				foreach (XmlNode node in ToXmlDoc(path))
				{
					TopLevelGroups.Add(BuildActionGroup(node));
				}
			}
			return TopLevelGroups;
		}

		//private static Script BuildScript(string path, string title, Execution.Parameter[] parameters)
		//{
		//	var srcCode = ToCSCode(path);
		//	var paramsLiteral = new StringBuilder();
		//	for (int i = 0; i < parameters.Length; i++)
		//	{
		//		// code gen part
		//		// result is like "(LCGuidebook.Core.Datastructure.Platform.Type)lcgGlobalVar[0], ..."
		//		paramsLiteral.Append($"({parameters[i].ObjectType.ToString().Replace('+', '.')}){nameof(Global.LcgGlobalVars)}[{i}]");
		//		if (i != parameters.Length - 1)
		//		{
		//			paramsLiteral.Append(", ");
		//		}
		//	}

		//	Script script = CSharpScript.Create($"{srcCode}\n{title}({paramsLiteral})", 
		//		ScriptOptions.Default.WithReferences(typeof(Superpower).Assembly), typeof(Global));
		//	return script;
		//}

		//private static Execution.Parameter BuildParameter(XmlNode node)
		//{
		//	var name = node.Attributes["displayName"].Value;
		//	var type = node.Attributes["type"].Value;
		//	var description = node.Attributes["description"]?.Value;

		//	return new Execution.Parameter(type, name, description);
		//}

		//private	static Delegate BuildBody(XmlNode node, Execution.Parameter[] parameters)
		//{
		//	var src = GeneratePath("library", "commands", node.Attributes["src"].Value);
		//	var title = node.Attributes["title"].Value;

		//	var script = BuildScript(src, title, parameters);

		//	Action<object[]> callBack = args => script.RunAsync(new Global(args));
		//	return callBack;
		//}
	}
}
