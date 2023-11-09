using System.Collections.Generic;

namespace SyntaxEditor.Model
{
    public class Category
    {
        public string Name { get; set; }
        public bool Expanded { get; set; }
        public IReadOnlyList<Property> Properties { get; set; }
    }
}
