using Sericaer.UIBind.Runtime.Core;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using UnityEngine.UI;

namespace Sericaer.UIBind.Runtime
{
    public class ButtonBinder : AbstractBinder<ButtonBinder.PropertyEnum, Button>
    {
        public enum PropertyEnum
        {
            [BindWay(nameof(OnCommandChanged))]
            Command,
        }

        private ICommand command;

        protected override void Awake()
        {
            base.Awake();

            target.onClick.AddListener(() => { command.Exec(); });
        }

        void OnCommandChanged(ICommand command)
        {
            if(this.command != null)
            {
                command.PropertyChanged -= CommandEnableChanged;
            }

            this.command = command;
            command.PropertyChanged += CommandEnableChanged;
        }

        private void CommandEnableChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ICommand.isEnable))
            {
                target.interactable = command.isEnable;
            }
        }
    }

    public interface ICommand : INotifyPropertyChanged
    {
        bool isEnable { get; }
        void Exec();
    }
}
