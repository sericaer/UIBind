using System;

namespace Sericaer.UIBind.Runtime.Core
{
    public enum BindWay
    {
        OneWay,
        TwoWay
    }

    public class BindWayAttribute : Attribute
    {
        public readonly BindWay bindWay;
        public readonly string updateTargetMethod;

        public BindWayAttribute(string updateTargetMethod,  BindWay bindWay = BindWay.OneWay)
        {
            this.bindWay = bindWay;
            this.updateTargetMethod = updateTargetMethod;
        }
    }
}