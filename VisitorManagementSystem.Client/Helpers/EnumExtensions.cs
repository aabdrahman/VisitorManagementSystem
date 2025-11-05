using System.ComponentModel;
using System.Reflection;

namespace VisitorManagementSystem.Client.Helpers;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Length > 0)
        {
            return attributes[0].Description;
        }
        else
        {
            return value.ToString(); // Return the enum member name if no description is found
        }
    }
}
