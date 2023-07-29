using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace SyntaxEditor.Controls
{
    class ComboPanel : TabControl
    {
        public ObservableCollection<string> DisplayItems { get; } = new ObservableCollection<string>();

        public ComboPanel()
        {
            this.Loaded += ComboPanel_Loaded;
            this.Unloaded += ComboPanel_Unloaded;
        }

        private void ComboPanel_Loaded(object sender, RoutedEventArgs e)
        {
            ((INotifyCollectionChanged)this.Items).CollectionChanged += ComboPanel_CollectionChanged;

            foreach (TabItem item in this.Items)
            {
                DisplayItems.Add(item.Header.ToString());
            }
        }

        private void ComboPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            ((INotifyCollectionChanged)this.Items).CollectionChanged -= ComboPanel_CollectionChanged;
            DisplayItems.Clear();
        }

        private void ComboPanel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (TabItem oldItem in e.OldItems)
                {
                    DisplayItems.Remove(oldItem.Header.ToString());
                }
            }
            if (e.NewItems != null)
            {
                foreach (TabItem newItem in e.NewItems)
                {
                    DisplayItems.Add(newItem.Header.ToString());
                }
            }
        }
    }

    internal class ComboPanelItem : TabItem
    {

    }

}
