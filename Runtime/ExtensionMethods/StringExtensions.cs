using UnityEngine;

namespace MUtility
{
public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s)
        => string.IsNullOrEmpty(s);

    public static bool NotNullOrEmpty(this string s)
        => !s.IsNullOrEmpty();
    
    public static string Colored(this string message, Color color) => $"<color={color.ToHex()}>{message}</color>";

}
}