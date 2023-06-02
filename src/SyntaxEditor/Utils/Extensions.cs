using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SyntaxEditor.Utils
{
    public static class ObservableExtension
    {
        public static ObservableCollection<TSource> ToObservable<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source.ToList());
        }
    }
}
