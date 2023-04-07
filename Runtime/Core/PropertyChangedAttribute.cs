using System;

namespace Sericaer.UIBind.Runtime.Core
{
    public class PropertyChangedAttribute : Attribute
    {
        public readonly object propertyEnum;

        public PropertyChangedAttribute(object propertyEnum)
        {
            this.propertyEnum = propertyEnum;
        }
    }
}