using GraphShape.Utils;
using System.Windows.Controls;

namespace SyntaxEditor.ViewModel
{
    public class DocumentBrowser : NotifierObject
    {

        public Orientation Orientation { get; set; }

    }

    namespace Sample
    {
        public class DocumentBrowser : ViewModel.DocumentBrowser
        {
            public DocumentBrowser() : base()
            {
            }
        }
    }
}
