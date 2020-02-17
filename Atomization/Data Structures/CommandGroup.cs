using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomization.DataStructures
{
	// TODO: Move all nested static methods into dynamically generated groups
	// This class is set up for future xml serialization
	public class CommandGroup : IUniqueObject
	{
		public CommandGroup(string name, params Command[] commands)
		{
			Name = name;
			Commands = commands.ToList();
		}

		public string Name { get; set; }
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
		public Command(string name, Delegate body)
		{
			Name = name;
			Body = body;
		}

		public string Name { get; set; }
		public string UID { get; } = GameManager.GenerateUID();

		public List<Parameter> Signature { get; } = new List<Parameter>();
		public Delegate Body { get; }

		public void Execute() => Body.DynamicInvoke(Signature.ToArray());

		public class Parameter
		{
			public Parameter(string typeName)
			{
				ObjectType = Type.GetType(typeName);
			}

			public readonly Type ObjectType;
			public object ObjectData { get; set; }
		}
	}
}
