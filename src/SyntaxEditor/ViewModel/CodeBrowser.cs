using System.Collections.Generic;
using System.Linq;
using GraphShape.Utils;
using SyntaxEditor.Model;
using P = System.Reflection.BindingFlags;
using AST = TypeScript.Syntax.Node;
using System.Drawing;

namespace SyntaxEditor.ViewModel
{

    public class CodeBrowser : NotifierObject
    {
        public IReadOnlyList<TextBlock> Source { get; set; }
        public Point Cursor { get; set; }

        private AST _AST;
        public AST AST
        {
            get => _AST;
            set
            {
                _AST = value;
                Source = ConvertToTextBlock(new State(), value).ToList();
                Cursor = Point.Empty;
                OnPropertyChanged(nameof(AST));
                OnPropertyChanged(nameof(Source));
                OnPropertyChanged(nameof(Cursor));
            }
        }

        private IEnumerable<TextBlock> ConvertToTextBlock(State st, AST AST)
        {
            var kind = AST.Kind.ToString();
            st.Position.Offset(1, 0);
            if (kind == "NewLineTrivia")
            {
                st.Position.Offset(-st.Position.X, 1);
            }
            yield return new TextBlock(
                kind: kind,
                text: AST.Text,
                position: st.Position,
                ast: AST
            );
            foreach (var i in AST.Children
                    .SelectMany(c => ConvertToTextBlock(st, c)))
                yield return i;
        }

        private class State
        {
            public Point Position { get; set; }
        }
    }

    namespace Sample
    {
        public class CodeBrowser : ViewModel.CodeBrowser
        {
            public CodeBrowser() : base()
            {
                AST = new SyntaxBrowser().AST;
            }
        }
    }

}
