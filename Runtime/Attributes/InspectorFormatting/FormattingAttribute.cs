using System;
using System.Reflection;
using UnityEngine;

namespace MUtility
{
[AttributeUsage(AttributeTargets.Field)]
public class FormattingAttribute : PropertyAttribute
{
    protected const BindingFlags bindings =
        BindingFlags.Instance |
        BindingFlags.Static |
        BindingFlags.Public |
        BindingFlags.NonPublic;
}
}