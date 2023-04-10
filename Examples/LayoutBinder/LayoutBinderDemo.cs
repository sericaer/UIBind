using Sericaer.UIBind.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LayoutBinderDemo : MonoBehaviour
{
    class TestData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<INotifyPropertyChanged> items { get; } = new ObservableCollection<INotifyPropertyChanged>();

        public class Item : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public int count 
            { 
                get
                {
                    return _count;
                }
                set
                {
                    _count = value;

                    NotifyPropertyChanged();
                }
            }

            private int _count;

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public BindContext bc;

    private TestData testData;

    private void Awake()
    {
        testData = new TestData();
        bc.SetContextData(testData);
    }


    int count;
    bool isAdd = true;

    void Update()
    {
        if (count % 100 == 0)
        {
            if(testData.items.Count == 0)
            {
                isAdd = true;
            }
            else if(testData.items.Count > 10)
            {
                isAdd = false;
            }

            if(isAdd)
            {
                foreach (TestData.Item item in testData.items)
                {
                    item.count++;
                }

                testData.items.Add(new TestData.Item());
            }
            else
            {
                testData.items.RemoveAt(0);
            }
        }

        count++;
    }
}
