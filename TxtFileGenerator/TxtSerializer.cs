using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace TxtFileGenerator
{
    public class TxtSerializer : ITxtSerializer
    {
        public string Serialize(object obj)
        {
            var sb = new StringBuilder();
            var type = obj.GetType();

            var txtRegisterName = type.GetCustomAttributes(typeof(TxtRegisterName), false).Cast<TxtRegisterName>().SingleOrDefault();

            var properties = type.GetProperties();

            var values = new List<string>();

            if (txtRegisterName != null)
                values.Add(txtRegisterName.Name);

            foreach (var property in properties)
            {
                if(property.GetCustomAttributes(typeof(TxtIgnoreProperty), false).Any()) 
                    continue;

                var txtPropertyTypeValue = property.GetCustomAttributes(typeof(TxtPropertyTypeValue), false).Cast<TxtPropertyTypeValue>().SingleOrDefault();

                var propertyType = property.PropertyType;

                var @interface = propertyType.GetInterface("ienumerable", true);

                var value = property.GetValue(obj, null);

                if(value == null)
                {
                    values.Add("");
                    continue;
                }

                if (txtPropertyTypeValue != null)
                {
                    var typeConverter = TypeDescriptor.GetConverter(txtPropertyTypeValue.Type);
                    values.Add(typeConverter.ConvertToString(value));
                    continue;
                }

                if (propertyType.IsPrimitive || typeof(string).IsAssignableFrom(propertyType))
                    values.Add(value.ToString());
                else if (propertyType.IsEnum)
                {
                    values.Add(Convert.ToInt32(value).ToString());
                }
                else if (@interface != null)
                {
                    var enumerable = value as IEnumerable<object>;
                    if (enumerable != null)
                    {
                        foreach (var o in enumerable)
                            sb.Append("\n" + Serialize(o));
                    }
                }
                else if(propertyType.IsClass)
                {
                    sb.AppendLine(Serialize(value));
                }
            }
            var result = "";
            if (values.Any()) 
                result += string.Format("|{0}|", string.Join("|", values));
            if(sb.Length > 0)
                result += sb;
            return Regex.Replace(result, @"^[|]{2,}", "|", RegexOptions.Multiline);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TxtRegisterName : Attribute
    {
        public string Name { get; set; }

        public TxtRegisterName(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TxtIgnoreProperty : Attribute
    {}

    [AttributeUsage(AttributeTargets.Property)]
    public class TxtPropertyTypeValue : Attribute
    {
        public Type Type { get; set; }

        public TxtPropertyTypeValue(Type type)
        {
            Type = type;
        }
    }
}
