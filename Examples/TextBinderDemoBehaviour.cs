using Sericaer.UIBind.Runtime;
//using Sericaer.UIBind.Runtime.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Sericaer.UIBind.Examples.TextBinderDemo
{
    public class TextBinderDemoBehaviour : MonoBehaviour
    {
        private TestData data1;
        private TestData data2;

        public BindContext bc1;
        public BindContext bc2;

        private void Start()
        {
            data1 = new TestData();
            data2 = new TestData();

            //BindCore.SetContext("KEY1", data1);
            bc1.SetContextData(data1);
            bc2.SetContextData(data2);

            //BindCore.SetContext("KEY2", data2);
        }

        int count;
        void Update()
        {
            if(count % 100 == 0)
            {
                data1.intValue = count/100;
                data1.strValue = $"DATA1 is {data1.intValue}";

                data2.intValue = data1.intValue * 2;
                data2.strValue = $"DATA2 is {data2.intValue}";

                data2.strFontSize = (data2.strFontSize + 1) % 20;
            }

            count++;
        }
    }

    class TestData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int intValue
        {
            get
            {
                return _intValue;
            }
            set
            {
                if(_intValue != value)
                {
                    _intValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int strFontSize
        {
            get
            {
                return _strFontSize;
            }
            set
            {
                if (_strFontSize != value)
                {
                    _strFontSize = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string strValue
        {
            get
            {
                return _strValue;
            }
            set
            {
                if(_strValue != value)
                {
                    _strValue = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _intValue;
        private int _strFontSize = 10;
        private string _strValue;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
