// The `#r` directives exist just for Visual Studio IntelliSense. 
// They will be removed before the script being interpreted
#r "../../../Core/bin/debug/netcoreapp3.1/Core.dll"
#r "System.Runtime.dll"
#r "System.Collections.dll"

// The essential usings
using System;
using System.Collections.Generic;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

// A typical property. The `public` keyword doesn't really matter, 
// as long as it is described in the xml header file
// because it will be retrived by using script.ContinueWith("SomeProperty");
// Note that the default setter will never be called
public string SomeProperty { get; set; }

// A typical script will have some methods. 
// Note that if it's an intended functionality, it should be described in the xml header file, 
// as well as using `string[]` as return value 
// in order to inform the action group the properties that are changed during method call
string[] Example0(int hello, string world)
{
	// In this case, I changed nothing so the array is empty
	return new string[0];
}

string[] Example1(int change, bool something)
{
	SomeProperty = change.ToString();

	// `SomeProperty` is changed
	return new string[] { nameof(SomeProperty) };
}

// On the other hand, the set functionality must be provided via this method
// Therefore, this method is only optional when there is no property in the script, 
// otherwise, use the exact signature as shown to make sure `ActionGroup` can set values
// and reflect the changes correctly
string[] Set(string propertyName, object value)
{
	switch (propertyName)
	{
		case nameof(SomeProperty):
			SomeProperty = (string)value;
			break;

		default:
			throw new InvalidOperationException();
	}

	return new string[] { nameof(SomeProperty) };
}