using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDesigner_XML.Common
{
    public class TrulyObservableCollection<T> : ObservableCollection<T>
where T : INotifyPropertyChanged
    {

        // A delegate type for hooking up change notifications.
        public delegate void ChangedEventHandler(object sender, EventArgs e);


        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event ChangedEventHandler ItemChanged;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChanged(object sender, EventArgs e)
        {
            if (ItemChanged != null)
                ItemChanged(sender, e);
        }

        public TrulyObservableCollection()
            : base()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(TrulyObservableCollection_CollectionChanged);
        }

        public TrulyObservableCollection(IList<T> itemsToAdd)
            : this()
        {
            CollectionChanged += new NotifyCollectionChangedEventHandler(TrulyObservableCollection_CollectionChanged);
            foreach (var item in itemsToAdd)
            {
                this.Add(item);
            }   
        }

        void TrulyObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
                }
            }
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnChanged(sender, e);
        }
    }
}
