using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
                var propertyType = property.PropertyType;

                var @interface = propertyType.GetInterface("ienumerable", true);

                if (propertyType.IsPrimitive || typeof(string).IsAssignableFrom(propertyType))
                    values.Add(property.GetValue(obj, null).ToString());
                else if (@interface != null)
                {
                    var enumerable = property.GetValue(obj, null) as IEnumerable<object>;
                    if (enumerable != null)
                    {
                        foreach (var o in enumerable)
                            sb.Append(Environment.NewLine + Serialize(o));
                    }
                }
            }

            return string.Format("|{0}|", string.Join("|", values)) + sb;
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
}
