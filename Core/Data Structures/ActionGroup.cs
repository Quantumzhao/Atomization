using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using UIEngine;

namespace LCGuidebook.Core.DataStructures
{
	public class ActionGroup : IUniqueObject, IVisible
	{
		public ActionGroup(string name, string description = "")
		{
			Name = name;
			Description = description;
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public string UID { get; } = GameManager.GenerateUID();
		public string Header => Name;
		public ScriptState ScriptState { get; set; }

		[Visible(nameof(Actions))]
		public List<AbstractAction> Actions { get; } = new List<AbstractAction>();
		[Visible(nameof(SubGroups))]
		public List<ActionGroup> SubGroups { get; } = new List<ActionGroup>();

		public void AddAction(Execution command)
		{
			Actions.Add(command);
		}

		public void AddSubGroup(ActionGroup group)
		{
			SubGroups.Add(group);
		}
	}

	public abstract class AbstractAction : IUniqueObject, IVisible
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public string Header { get; set; }

		public string UID { get; } = GameManager.GenerateUID();
	}

	public class Bulletinboard : AbstractAction
	{
		public object ShadowObject { get; set; }
	}

	public class Execution : AbstractAction
	{
		public Execution(string name, string description = "")
		{
			Name = name;
			Description = description;
		}

		public Parameter[] Signature { get; set; }

		public Delegate Body { get; set; }

		public void AssignArgument(object value, int index)
		{
			Signature[index].ObjectData = value;
		}

		public void Execute()
		{
			Body.DynamicInvoke(new object[] { Signature.Select(p => p.ObjectData).ToArray() });
		}

		public class Parameter
		{
			public Parameter(string typeName, string name, string description = "")
			{
				Name = name;
				Description = description;
				ObjectType = Type.GetType(typeName);
			}

			public readonly Type ObjectType;
			public string Description { get; set; }
			public string Name { get; set; }
			public object ObjectData { get; set; } = null;
		}
	}
}
