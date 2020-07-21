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

		/// <summary>
		///		The name that will be displayed on the UI. 
		/// </summary>
		public string Name { get; set; }

		public string Description { get; set; }

		public string Header { get; set; }

		public string UID { get; } = GameManager.GenerateUID();

		public ActionGroup Group { get; private set; }
	}

	public class Field : AbstractAction, INotifyPropertyChanged, IActionGroupField
	{
		public Field(ActionGroup parent, string name, string bindedFieldName) : base(parent)
		{
			Name = name;
			BindedFieldName = bindedFieldName;
		}

		public static object TempRightValue { get; private set; }

		public string BindedFieldName { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public object Value
		{
			get => Group.Script.ContinueWith($"{BindedFieldName}\n").RunAsync().Result.ReturnValue;
			set
			{
				TempRightValue = value;
				string[] changedProperties = Group.Script.ContinueWith(
					$"Set({BindedFieldName}, LCGuidebook.Core.DataStructures.Field.{nameof(TempRightValue)})\n")
					.RunAsync().Result.ReturnValue as string[];
				Group.RefreshFields(changedProperties);
				TempRightValue = null;
			}
		}

		public void InvokePropertyChanged() 
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
	}

	public class Execution : AbstractAction, IActionGroupExecution
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
	}
}
