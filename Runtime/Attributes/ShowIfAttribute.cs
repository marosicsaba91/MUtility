using System; 
using UnityEngine;  

namespace MUtility
{

[AttributeUsage(AttributeTargets.Field)]
public class FormatAttribute : PropertyAttribute
{
	public readonly string[] conditionMembers;
 
	public FormatAttribute(string elementToCheck)
		=> conditionMembers = conditionMembers = new[] { elementToCheck };

	public FormatAttribute(params string[] conditionMembers)
		=> this.conditionMembers = conditionMembers;

	public bool IsConditionMatch(object owner) => ConditionHelper.CheckConditions(owner, conditionMembers);
} 
}