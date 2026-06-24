namespace Project.Core.Extension
{
    using System.ComponentModel;
    using System.Reflection;
    using Project.Core.Enum;

    // / <summary>
    // / String Extension.
    // / </summary>
    public static class StringExtension
    {
        // / <summary>
        // / GetDescription.
        // / </summary>
        // / <param name="value">EnumRole.</param>
        // / <returns>string.</returns>
        public static string GetDescription(this EnumRole value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
            {
                return null;
            }
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
       
            public static void TrimStringProperties(object obj)
            {
                // Get all properties of the object's type
                PropertyInfo[] properties = obj.GetType().GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    // Check if the property is of type string
                    if (property.PropertyType == typeof(string))
                    {
                        // Get the current value of the property
                        string value = (string)property.GetValue(obj);

                        // Trim the value if it's not null
                        if (value != null)
                        {
                            property.SetValue(obj, value.Trim());
                        }
                    }
                }
            }
        


    }
}