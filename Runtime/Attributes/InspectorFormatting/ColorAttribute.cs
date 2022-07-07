namespace MUtility
{
public class ColorAttribute : FormattingAttribute
{
    public string colorMember;

    public ColorAttribute(string colorMemberName)
        => colorMember = colorMemberName;
}
}