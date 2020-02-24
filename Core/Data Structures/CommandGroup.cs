using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace LCGuidebook.Core.DataStructures
{
	// TODO: Move all nested static methods into dynamically generated groups
	// This class is set up for future xml serialization
	public class CommandGroup : IUniqueObject
	{
		public CommandGroup(string name, string description = "")
		{
			Name = name;
			Description = description;
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public string UID { get; } = GameManager.GenerateUID();

		public readonly List<Command> Commands;

		public readonly List<CommandGroup> SubGroups = new List<CommandGroup>();

		public void AddCommand(Command command)
		{
			Commands.Add(command);
		}

		public void AddSubGroup(CommandGroup group)
		{
			SubGroups.Add(group);
		}
	}

	public class Command : IUniqueObject
	{
		public Command(string name, string description = "")
		{
			Name = name;
			Description = description;
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public string UID { get; } = GameManager.GenerateUID();

		public List<Parameter> Signature { get; } = new List<Parameter>();
		public Delegate Body { get; set; }

		public void Execute() => Body.DynamicInvoke(Signature.ToArray());

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
