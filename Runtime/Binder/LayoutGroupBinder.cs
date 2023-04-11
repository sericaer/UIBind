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
    public class LayoutGroupBinder : AbstractBinder<LayoutGroupBinder.PropertyEnum, LayoutGroupBinder>
    {
        public enum PropertyEnum
        {
            [BindWay(nameof(OnItemSourceChanged))]
            ItemSource,
        }

        public BindContext itemTemplate;

        private ObservableCollection<INotifyPropertyChanged> obsCollection;

        void OnItemSourceChanged(ObservableCollection<INotifyPropertyChanged> obsCollection)
        {
            if(this.obsCollection != null)
            {
                this.obsCollection.CollectionChanged -= OnCollectionChanged;
            }

            this.obsCollection = obsCollection;
            obsCollection.CollectionChanged += OnCollectionChanged;

            var oldItems = GetComponentsInChildren<BindContext>()
                .Where(x => x != itemTemplate)
                .ToArray();

            OnItemRemove(oldItems.Select(x => x.GetContextData()).ToList());

            OnItemAdd(obsCollection);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnItemAdd(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnItemRemove(e.OldItems);
                    break;
                default:
                    throw new NotImplementedException($"{this.name} not support obsCollection action:{e.Action}");
            }
        }

        private void OnItemRemove(IList oldItems)
        {
            var itemContexts = GetComponentsInChildren<BindContext>();
            foreach (var item in oldItems)
            {
                var oldContext = itemContexts.SingleOrDefault(x => x.GetContextData() == item);
                if (oldContext != null)
                {
                    Destroy(oldContext.gameObject);
                }
            }
        }

        private void OnItemAdd(IList newItems)
        {
            foreach(var item in newItems)
            {
                var newConext = Instantiate(itemTemplate, this.transform);
                newConext.SetContextData(item as INotifyPropertyChanged);
            }
        }
    }
}
