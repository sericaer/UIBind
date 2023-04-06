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
            //BindCore.SetContext("KEY1", data1);
            bc1.target = data1;

            data2 = new TestData();
            bc2.target = data2;

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
                data2.strValue = $"DATA1 is {data2.intValue}";
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
        private string _strValue;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
