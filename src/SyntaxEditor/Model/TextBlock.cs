using System;
using System.Collections.Generic;
using System.Drawing;

namespace SyntaxEditor.Model
{
    public class TextBlock
    {
        public string Text { get; set; }
        public Point Position { get; set; }
        public object AST { get; set; }
        public Color Color { get; set; }

        public TextBlock(string kind, string text, Point position, object ast)
        {
            Text = text;
            Position = position;
            AST = ast;
            Color = Table.KindColor.TryGetValue(kind, out var v) ? v : System.Drawing.Color.Black;
        }
    }
}
