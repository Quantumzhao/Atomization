using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using LCGuidebook;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LCGuidebook.Core.DataStructures
{
	public interface IExecutable
	{
		void Execute();
	}

	public interface IDeployable : IDestroyable
	{
		bool IsActivated { get; set; }
		Region DeployedRegion { get; set; }
	}

	public interface IDestroyable : IUniqueObject
	{
		event Action SelfDestroyed;
		void DestroyThis();
	}

	//public interface IBuildable : IDestroyable
	//{
	//	Effect BuildCost_LongTerm { get; }
	//}

	public interface IUniqueObject
	{
		string UID { get; }
	}

	public interface IActionGroup
	{

	}
}