using System;

namespace MUtility
{
[AttributeUsage(AttributeTargets.Field)]
public class ChangeCallbackAttribute : FormattingAttribute
{
	public string callbackMember;

	public ChangeCallbackAttribute(string callbackName)
		=> callbackMember = callbackName;
 
}
}