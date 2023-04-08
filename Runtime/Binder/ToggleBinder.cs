using Sericaer.UIBind.Runtime.Core;
using System;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class ToggleBinder : AbstractBinder<ToggleBinder.PropertyEnum, Toggle>
    {
        public enum PropertyEnum
        {
            [BindWay(nameof(OnValueChanged), BindWay.TwoWay)]
            Value,
        }

        void OnValueChanged(bool currValue)
        {
            target.isOn = currValue;
        }

        protected override void Awake()
        {
            base.Awake();

            target.onValueChanged.AddListener(OnUIValueChanged);
        }

        private void OnUIValueChanged(bool uiValue)
        {
            UpdateSource(PropertyEnum.Value, uiValue);
        }
    }
}
