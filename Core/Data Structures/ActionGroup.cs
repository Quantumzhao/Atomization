using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis.Scripting;
using UIEngine;

namespace LCGuidebook.Core.DataStructures
{
	public class ActionGroup : IUniqueObject, IVisible, INotifyPropertyChanged
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
		public Script Script { get; set; }

		[Visible(nameof(Actions))]
		public List<AbstractAction> Actions { get; } = new List<AbstractAction>();
		[Visible(nameof(SubGroups))]
		public List<ActionGroup> SubGroups { get; } = new List<ActionGroup>();

		public event PropertyChangedEventHandler PropertyChanged;

		public void AddAction(AbstractAction action)
		{
			Actions.Add(action);
		}

		public void AddSubGroup(ActionGroup group)
		{
			SubGroups.Add(group);
		}

		internal void RefreshFields(string[] names)
		{
			foreach (var propertyName in names)
			{
				(Actions.Single(a => a.Name == propertyName) as Field).InvokePropertyChanged();
			}
		}
	}

	public abstract class AbstractAction : IUniqueObject, IVisible
	{
		public AbstractAction(ActionGroup parent)
		{
			Group = parent;
		}

		public string Name { get; set; }

		public string Description { get; set; }

		public string Header { get; set; }

		public string UID { get; } = GameManager.GenerateUID();

		public ActionGroup Group { get; private set; }
	}

	public class Field : AbstractAction, INotifyPropertyChanged
	{
		public Field(ActionGroup parent) : base(parent) { }
		public static object TempRightValue { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		private object _Value;
		public object Value
		{
			get => Group.Script.ContinueWith($"{Name}\n").RunAsync().Result.ReturnValue;
			set
			{
				TempRightValue = value;
				string[] changedProperties = Group.Script.ContinueWith(
					$"Set({Name}, LCGuidebook.Core.DataStructures.Bulletinboard.{nameof(TempRightValue)})\n")
					.RunAsync().Result.ReturnValue as string[];
				Group.RefreshFields(changedProperties);
				TempRightValue = null;
			}
		}

		public void InvokePropertyChanged() 
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
	}

	public class Execution : AbstractAction
	{
		public Execution(ActionGroup parent, string name, string bindedMethodName, string description = "")
			: base(parent)
		{
			Name = name;
			Description = description;
			BindedMethodName = bindedMethodName;
		}

		public string BindedMethodName { get; set; }

		public void Execute()
		{
			Group.Script.ContinueWith($"{Name}()\n;");
		}

		//public class Parameter
		//{
		//	public Parameter(string typeName, string name, string description = "")
		//	{
		//		Name = name;
		//		Description = description;
		//		ObjectType = Type.GetType(typeName);
		//	}

		//	public readonly Type ObjectType;
		//	public string Description { get; set; }
		//	public string Name { get; set; }
		//	public object ObjectData { get; set; } = null;
		//}
	}
}
