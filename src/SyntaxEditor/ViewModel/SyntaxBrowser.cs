using System;
using QuikGraph;
using System.Linq;
using GraphShape.Utils;
using SyntaxEditor.Model;
using System.Collections.Generic;

using Graph = QuikGraph.IBidirectionalGraph<SyntaxEditor.Model.Node, QuikGraph.SEdge<SyntaxEditor.Model.Node>>;

namespace SyntaxEditor.ViewModel    
{

    public class SyntaxBrowser : NotifierObject
    {
        private Node _AST;
        public Node AST
        {
            get => _AST;
            set
            {
                _AST = value;
                Root = new[] { value };
                SelectedAST = null;
                _Graph = new Lazy<Graph>(() => ConvertToGraph(value));
                OnPropertyChanged(nameof(AST));
                OnPropertyChanged(nameof(Root));
                OnPropertyChanged(nameof(SelectedAST));
                OnPropertyChanged(nameof(Graph));
            }
        }

        public IReadOnlyList<Node> Root { get; set; }

        public object SelectedAST { get; set; }

        private Lazy<Graph> _Graph { get; set; }
    
        public Graph Graph { get => _Graph?.Value; }

        private Graph ConvertToGraph(Node root) =>
            GetAllNodes(root)
                .ToBidirectionalGraph(v =>
                    v.Nodes.Select(o => new SEdge<Node>(v, o)))
                .ToArrayBidirectionalGraph();

        private static IEnumerable<Node> GetAllNodes(Node root)
        {
            var visited = new HashSet<Node>();
            var stack = new Stack<Node>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var node = stack.Pop();
                if (!visited.Contains(node))
                {
                    visited.Add(node);
                    foreach (var childNode in node.Nodes)
                    {
                        stack.Push(childNode);
                    }
                }
            }
            return visited;
        }

    }

    namespace Sample
    {
        using SyntaxEditor.Utils;
        using static SyntaxEditor.Utils.Converter;

        public class SyntaxBrowser : ViewModel.SyntaxBrowser
        {
            private const string basePath = "demo/ast";
            private const string docNamespace = "Bailey";
            private const Lang outputLang = Lang.CSharp;
            private static string[] usings = new[] { "System.Linq", "TypeScript.CSharp" };
            private Converter converter = new Converter(
                basePath: basePath,
                docNamespace: docNamespace,
                outputLang: outputLang,
                usings: usings);

            public SyntaxBrowser() : base()
            {
                AST = converter.ConvertToNode();
            }
        }
    }
}
