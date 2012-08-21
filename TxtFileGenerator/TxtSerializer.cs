using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Linq;

namespace TxtFileGenerator
{
    public class TxtSerializer : ITxtSerializer
    {
        public string Serialize(object obj)
        {
            var sb = new StringBuilder();
            var values = new List<string>();
            var type = obj.GetType();

            var txtRegisterName = type.GetAttribute<TxtRegisterName>();

            if (txtRegisterName != null)
                values.Add(txtRegisterName.Name);

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (MustIgnore(property))
                    continue;

                var propertyTypeValue = property.GetAttribute<TxtPropertyTypeValue>();

                var propertyType = property.PropertyType;

                var value = property.GetValue(obj, null);

                if (value == null)
                {
                    values.Add(string.Empty);
                    continue;
                }

                if (propertyTypeValue != null)
                {
                    var typeConverter = TypeDescriptor.GetConverter(propertyTypeValue.Type);
                    values.Add(typeConverter.ConvertToString(value));
                    continue;
                }

                if (IsPrimitiveOrString(propertyType))
                    values.Add(value.ToString());
                else if (propertyType.IsEnum)
                    values.Add(Convert.ToInt32(value).ToString());
                else if (propertyType.IsEnumerable()) 
                    foreach(var item in (IEnumerable)value) 
                        sb.AppendLine(Serialize(item));
                else if(propertyType.IsClass) 
                    sb.AppendLine(Serialize(value));
            }

            var result = string.Empty;
            
            if (values.Any())
                result += string.Format("|{0}|", string.Join("|", values));
            
            if (sb.Length > 0 && result.Length > 0)
                result += Environment.NewLine + sb;
            else
                result += sb;
            
            return result;
        }

        private static bool IsPrimitiveOrString(Type propertyType)
        {
            return propertyType.IsPrimitive || typeof(string).IsAssignableFrom(propertyType);
        }

        private static bool MustIgnore(PropertyInfo property)
        {
            return property.GetCustomAttributes(typeof(TxtIgnoreProperty), false).Any();
        }
    }
}
