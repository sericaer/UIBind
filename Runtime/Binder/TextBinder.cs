using Sericaer.UIBind.Runtime.Core;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class TextBinder : AbstractBinder<TextBinder.PropertyEnum, Text>
    {
        public enum PropertyEnum
        {
            [BindWay(nameof(OnValueChanged), BindWay.OneWay)]
            Value,

            [BindWay(nameof(OnFontSizeChanged), BindWay.OneWay)]
            FontSize,
        }
        
        void OnValueChanged(string currValue)
        {
            target.text = currValue;
        }

        void OnFontSizeChanged(int currSize)
        {
            target.fontSize = currSize;
        }
    }
}
