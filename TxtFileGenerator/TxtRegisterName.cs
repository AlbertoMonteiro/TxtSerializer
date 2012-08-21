using System;

namespace TxtFileGenerator
{
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