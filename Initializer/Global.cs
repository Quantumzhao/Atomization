using LCGuidebook.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LCGuidebook.Initializer
{
	/// <summary>
	///		This class is designed for exchanging data between scripts and domain model
	/// </summary>
	public class Global
	{
		public Global(object[] args) => LcgGlobalVars = args;
		public object[] LcgGlobalVars;
	}
}
