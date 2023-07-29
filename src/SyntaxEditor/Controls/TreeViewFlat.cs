using System.Windows;
using System.Windows.Controls;

namespace SyntaxEditor.Controls
{

    class TreeViewFlat : TreeView
    {
        public TreeViewFlat()
        {
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeViewItemFlat();
        }
    }

    class TreeViewItemFlat : TreeViewItem
    {
        public TreeViewItemFlat()
        {
        }
    }
}
