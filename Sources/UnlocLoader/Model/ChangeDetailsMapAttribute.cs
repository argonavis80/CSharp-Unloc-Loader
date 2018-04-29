using System;

namespace UnlocLoader.Model
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ChangeDetailsMapAttribute : Attribute
    {
        public ChangeDetailsMapAttribute(string tag)
        {
            Tag = tag;
        }

        public string Tag { get; set; }
    }
}