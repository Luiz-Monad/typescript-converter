using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxEditor.Utils;

namespace SyntaxEditor.Model
{
    public class Document
    {
        public Node Source { get; set; }
        public Node Target { get; set; }
        public string Name { get; internal set; }
        public string Path { get; internal set; }
        public List<Converter.ConvertedNode> Nodes { get; internal set; }
    }
}
