using System;
using System.Reflection;
using UnityEngine;

namespace MUtility
{
[AttributeUsage(AttributeTargets.Field)]
public class FormattingAttribute : PropertyAttribute
{
}
}