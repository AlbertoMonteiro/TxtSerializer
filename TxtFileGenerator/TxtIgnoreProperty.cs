using System;

namespace TxtFileGenerator
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TxtIgnoreProperty : Attribute
    {}
}