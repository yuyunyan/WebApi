using System;
using System.ComponentModel;

namespace Sourceportal.Utilities
{
    public static class EnumerationExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            
            //Enum is one value
            if (field != null)
            {
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                // return  
                return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
            }

            //Enum is bitwise
            else
            {
               string descriptions = "";
               foreach (Enum val in Enum.GetValues(value.GetType()))
                {
                    if (value.HasFlag(val))
                    {
                        field = enumType.GetField(val.ToString());
                        var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                        descriptions += attributes.Length == 0 ? value.ToString() + "," : ((DescriptionAttribute)attributes[0]).Description + ",";
                    }
                }
                return descriptions.TrimEnd(',');
            }
        }
    }
}
