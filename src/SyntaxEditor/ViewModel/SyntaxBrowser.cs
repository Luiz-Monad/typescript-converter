using System;
using System.Collections.Generic;
using System.Linq;
using GraphShape.Utils;
using SyntaxEditor.Model;
using P = System.Reflection.BindingFlags;
using AST = TypeScript.Syntax.Node;
using QuikGraph;
using Graph = QuikGraph.ArrayAdjacencyGraph<SyntaxEditor.Model.Node, QuikGraph.SEdge<SyntaxEditor.Model.Node>>;

namespace SyntaxEditor.ViewModel
{

    public class SyntaxBrowser : NotifierObject
    {
        private AST _AST;
        public AST AST
        {
            get => _AST;
            set
            {
                _AST = value;
                Root = ConvertToNode(value);
                SelectedAST = null;
                _Graph = new Lazy<Graph>(() => ConvertToGraph(Root));
                OnPropertyChanged(nameof(AST));
                OnPropertyChanged(nameof(Root));
                OnPropertyChanged(nameof(SelectedAST));
                OnPropertyChanged(nameof(Graph));
            }
        }

        public Node Root { get; set; }

        public object SelectedAST { get; set; }

        private Lazy<Graph> _Graph { get; set; }
        public Graph Graph { get => _Graph.Value; }

        private Node ConvertToNode(AST AST) => new Node()
        {
            Name = AST.NodeName,
            Kind = AST.Kind.ToString(),
            Text = AST.Text,
            Properties = AST.GetType()
                    .GetProperties(P.Instance | P.Public)
                    .Select(prop => new Property()
                    {
                        Name = prop.Name,
                        Value = prop.GetValue(AST)
                    })
                    .ToList(),
            Nodes = AST.Children
                    .Select(ConvertToNode)
                    .ToList(),
            AST = AST
        };

        private Graph ConvertToGraph(Node Root) =>
            (new[] { Root })
                .ToAdjacencyGraph(v =>
                    v.Nodes.Select(o => new SEdge<Node>(v, o)))
                .ToArrayAdjacencyGraph();

    }

    namespace Sample
    {
        using System.IO;
        using Newtonsoft.Json.Linq;
        using TypeScript.Converter.CSharp;
        using TypeScript.Syntax;

        public class SyntaxBrowser : ViewModel.SyntaxBrowser
        {
            public SyntaxBrowser() : base()
            {
                var basePath = "/";
                var builder = new AstBuilder();
                var documents = Data.Instance.Value.Select(
                    d => new Document(Path.Join(basePath, d.Key), (SourceFile)builder.Build(d.Value))
                    ).ToList();
                var project = new Project(basePath, documents, new List<Document>());
                var converterConfig = new ConverterConfig
                {
                    OmittedQualifiedNames = new List<string>(),
                    NamespaceMappings = new Dictionary<string, string>()
                };
                var convertContext = new ConverterContext(project, converterConfig);
                ConverterContext.Current = convertContext;
                var nodes = new List<Node>();
                foreach (var doc in documents)
                {
                    nodes.Add(doc.Root);
                }
                convertContext.Analyze(nodes);
                AST = nodes.First();
            }

            public class Data
            {
                public static Lazy<Dictionary<string, JToken>> Instance = new Lazy<Dictionary<string, JToken>>(() =>
                    new Dictionary<string, JToken>() {
                        { "Greeter.ts", ToJson(SyntaxEditor.Resources.Resources.Greeter_ts) },
                        { "IGreeter.ts", ToJson(SyntaxEditor.Resources.Resources.IGreeter_ts) }
                    }
                );

                private static JToken ToJson(byte[] content)
                {
                    using (var str = new System.IO.MemoryStream(content))
                    using (var sr = new System.IO.StreamReader(str))
                    {
                        return JToken.Parse(sr.ReadToEnd());
                    }
                }

            }
        }

    }

}
