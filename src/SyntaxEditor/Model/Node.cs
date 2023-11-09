﻿using System.Collections.Generic;

namespace SyntaxEditor.Model
{
    public class Node
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public string Text { get; set; }
        public IReadOnlyList<Property> Properties { get; set; }
        public IReadOnlyList<Node> Nodes { get; set; }
        public object AST { get; set; }
    }
}