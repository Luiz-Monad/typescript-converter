using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxEditor.Model
{
    public class Category
    {
        public string Name { get; set; }
        public bool Expanded { get; set; }
        public IReadOnlyList<Property> Properties { get; set; }
    }
}
