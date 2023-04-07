using Sericaer.UIBind.Runtime;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ToggleBinderDemo : MonoBehaviour
{
    public BindContext bc1;
    public BindContext bc2;

    private TestData data1;

    private TestData data2;

    // Start is called before the first frame update
    void Start()
    {
        data1 = new TestData();
        data2 = new TestData();

        bc1.SetContextData(data1);
        bc2.SetContextData(data2);
    }

    int count;
    void Update()
    {
        if (count % 100 == 0)
        {
            if(data1.overturnValue)
            {
                data2.overturnValue = !data2.overturnValue;
            }
        }

        count++;
    }

    class TestData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool overturnValue
        {
            get
            {
                return _overturnValue;
            }
            set
            {
                if (_overturnValue != value)
                {
                    _overturnValue = value;

                    NotifyPropertyChanged();
                }
            }
        }
        private bool _overturnValue;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
