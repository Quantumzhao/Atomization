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
