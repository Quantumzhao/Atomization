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

namespace LCGuidebook.Core
{
	//public delegate void StageFinishedHandler(Task previous, Task next, Task task);

	public static class Misc
	{
		public const string BAD_ACTIVATION_SEQUENCE = "Must be activated after all dependencies";

		public static int Round(double value) => (int)(value + 0.5);
	}
}
