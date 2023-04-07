using System;

namespace Sericaer.UIBind.Runtime.Core
{
    public enum BindWay
    {
        OneWay,
        TwoWay
    }

    public class PropertyChangedAttribute : Attribute
    {
        public readonly object propertyEnum;
        public readonly BindWay bindWay;

        public PropertyChangedAttribute(object propertyEnum, BindWay bindWay = BindWay.OneWay)
        {
            this.propertyEnum = propertyEnum;
            this.bindWay = bindWay;
        }
    }
}