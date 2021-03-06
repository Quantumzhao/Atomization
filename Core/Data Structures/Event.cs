﻿using System;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

namespace LCGuidebook.Core
{
	public delegate void TaskProgressAdvanceHandler(Task sender, TaskProgressAdvancedEventArgs e);
	public delegate void ValueUpdatedHandler(ValueComplex sender, ValueUpdatedEventArgs e);

	public abstract class GameEventArgs : EventArgs
	{
		public string Message { get; set; } = string.Empty;
		public ConfidentialLevel ConfidentialLevel { get; set; } = ConfidentialLevel.Domestic;
		public object Source { get; set; } = null;
		public Impact Influence { get; set; } = Impact.Undefined;
	}

	public class TaskProgressAdvancedEventArgs : GameEventArgs
	{
		private const string _NO_DESIGNATED_GAMEOBJECT = "No game object was specified";
		public TaskProgressAdvancedEventArgs(int timeRemaining)
		{
			TimeRemaining = timeRemaining;
			ConfidentialLevel = ConfidentialLevel.Secret;
		}

		public readonly int TimeRemaining;

		public string RelatedGameObjectUid { get; set; }
		public bool IsTaskFinished => TimeRemaining <= 0;
		public Task TaskInfo { get; set; }
	}

	public class ValueUpdatedEventArgs : GameEventArgs
	{
		
	}

	public enum ConfidentialLevel
	{
		/// <summary>
		///		Remain mostly unaware even within the central government
		/// </summary>
		TopSecret = 0,

		/// <summary>
		///		Opaque to the public
		/// </summary>
		Secret = 1,

		/// <summary>
		///		Not intentionally informing other nations
		/// </summary>
		Domestic = 2,

		/// <summary>
		///		Actively advertising
		/// </summary>
		Global = 3
	}

	public enum Impact
	{
		Positive, 
		Negative, 
		Undefined
	}
}