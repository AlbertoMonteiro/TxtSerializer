using System;

namespace TxtFileGenerator
{
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