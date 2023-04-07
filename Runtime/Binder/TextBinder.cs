using Sericaer.UIBind.Runtime.Core;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class TextBinder : AbstractBinder<TextBinder.PropertyEnum, Text>
    {
        public enum PropertyEnum
        {
            Value,
            FontSize,
        }
        
        [PropertyChanged(PropertyEnum.Value)]
        void OnValueChanged(string currValue)
        {
            target.text = currValue;
        }

        [PropertyChanged(PropertyEnum.FontSize)]
        void OnFontSizeChanged(int currSize)
        {
            target.fontSize = currSize;
        }
    }
}
