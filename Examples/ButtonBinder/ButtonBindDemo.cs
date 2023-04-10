using Sericaer.UIBind.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonBindDemo : MonoBehaviour
{
    class TestData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Command cmd1 { get; }
        public Command cmd2 { get; }

        public TestData()
        {
            cmd1 = new Command()
            {
                isEnable = true,
                DoExec = () =>
                {
                    Debug.Log("CMD1 EXEC");
                    cmd2.isEnable = !cmd2.isEnable;
                }
            };

            cmd2 = new Command()
            {
                isEnable = true,
                DoExec = () =>
                {
                    Debug.Log("CMD2 EXEC");
                }
            };
        }
    }

    class Command : ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool isEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;

                    NotifyPropertyChanged();
                }
            }
        }
        private bool _isEnable;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Action DoExec { get; set; }

        public void Exec()
        {
            DoExec?.Invoke();
        }
    }

    public BindContext bc;
    private TestData testData;

    // Start is called before the first frame update
    void Awake()
    {
        testData = new TestData();
        bc.SetContextData(testData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
