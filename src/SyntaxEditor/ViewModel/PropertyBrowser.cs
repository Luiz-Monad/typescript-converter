using System.Collections.Generic;
using System.Linq;
using GraphShape.Utils;
using SyntaxEditor.Model;
using P = System.Reflection.BindingFlags;

namespace SyntaxEditor.ViewModel
{

    public class PropertyBrowser : NotifierObject
    {
        private object _object;
        public object Object
        {
            get => _object;
            set
            {
                _object = value;
                Properties = value.GetType()
                    .GetProperties(P.FlattenHierarchy | P.Instance | P.Public)
                    .Select(prop => new Property()
                    {
                        Name = prop.Name,
                        Value = prop.GetValue(value),
                        Category = CategoryList.TryGetValue(prop.Name, out var v) ? v : null
                    })
                    .ToList();
                Categories = Properties
                    .GroupBy(prop => prop.Category)
                    .Select(cat => new Category()
                    {
                        Name = cat.Key,
                        Properties = cat.ToList()
                    })
                    .ToList();
                OnPropertyChanged(nameof(Object));
                OnPropertyChanged(nameof(Properties));
                OnPropertyChanged(nameof(Categories));
            }
        }

        public IReadOnlyList<Category> Categories { get; private set; }

        public IReadOnlyList<Property> Properties { get; private set; }

        protected Dictionary<string, string> CategoryList =
            new Dictionary<string, string>();

    }

    namespace Sample
    {
        public class PropertyBrowser : ViewModel.PropertyBrowser
        {
            public PropertyBrowser() : base()
            {
                CategoryList =
                    new Dictionary<string, string>()
                    {
                        { "Test", "Cat" },
                        { "Read", "Props" },
                        { "Write", "Props" }
                    };
                Object = Data.Instance;
                foreach (var cat in Categories)
                {
                    cat.Expanded = true;
                }
            }

            public class Data
            {
                public string Test { get; set; }
                public bool Read { get; set; }
                public bool Write { get; set; }

                public static Data Instance = new Data()
                {
                    Test = "Test",
                    Read = true,
                    Write = false
                };
            }
        }

    }

}
