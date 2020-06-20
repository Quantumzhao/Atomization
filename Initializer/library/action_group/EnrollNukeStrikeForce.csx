/* #r "../../../Core/bin/debug/netcoreapp3.1/Core.dll"
#r "System.Runtime.dll"
#r "System.Collections.dll"

using System;
using System.Collections.Generic;
using LCGuidebook.Core;
using LCGuidebook.Core.DataStructures;

string NO_SUCH_PROPERTY(string name) => $"The property \"{name}\" does not exist in the script";

public CostOfStage Cost { get; private set; } = new CostOfStage(new Effect(), new Effect(), (Expression)0);

public CarrierType Range { get; private set; }

public Warhead.Types Power { get; private set; }

public Platform.Types Concealment { get; private set; }

void UpdateCost()
{
	throw new NotImplementedException();
}

public string[] Enroll()
{

	return new string[0];
}

public string[] Set(string propertyName, object value)
{
	switch (propertyName)
	{
		case nameof(CarrierType):
			Range = (CarrierType)value;
			break;

		case nameof(Power):
			Power = (Warhead.Types)value;
			break;

		case nameof(Concealment):
			Concealment = (Platform.Types)value;
			break;

		default:
			throw new MemberAccessException(NO_SUCH_PROPERTY(propertyName));
	}

	UpdateCost();
	return new string[] { nameof(Cost) }; }*/
throw new System.NotImplementedException("This part is in Superpower.cs");
