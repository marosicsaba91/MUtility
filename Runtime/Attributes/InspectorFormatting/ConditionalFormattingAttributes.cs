using System;
using UnityEngine;

namespace MUtility
{
public class ConditionalFormattingAttribute : FormattingAttribute
{
	public readonly string[] conditionMembers;

	public ConditionalFormattingAttribute(params string[] conditionMembers) =>
		this.conditionMembers = conditionMembers;
}

public class ShowIfAttribute : ConditionalFormattingAttribute
{
	public ShowIfAttribute(params string[] conditionMembers) : base(conditionMembers) { }
}

public class HideIfAttribute : ConditionalFormattingAttribute
{ 
	public HideIfAttribute(params string[] conditionMembers) : base(conditionMembers) { }
}
public class EnableIfAttribute : ConditionalFormattingAttribute
{
	public EnableIfAttribute(params string[] conditionMembers) : base(conditionMembers) { }
}
public class DisableIfAttribute : ConditionalFormattingAttribute
{
	public DisableIfAttribute(params string[] conditionMembers) : base(conditionMembers) { }
}

public class ReadOnlyAttribute : FormattingAttribute { }


public static class FormattingAttributeHelper
{
	public static bool CheckConditions(this ConditionalFormattingAttribute attribute, object owner) =>
		CheckConditions(owner, attribute.conditionMembers);
 
	public static bool CheckConditions(object owner, string[] memberNames)
	{
		if (memberNames.IsNullOrEmpty()) return true;

		Type ownerType = owner.GetType();
		foreach (string memberName in memberNames)
		{
			if (memberName.IsNullOrEmpty())
				Debug.LogWarning($"No condition name in type {ownerType}");
			else if (InspectorDrawingUtility.TryGetAGetterFromMember(owner, memberName, out Func<bool> getter))
			{
				bool enabled = getter.Invoke();
				if(!enabled) return false;
			} 
			else
				Debug.LogWarning($"No member named {memberName} in type {ownerType}");
		}

		return true;
	}
}

}