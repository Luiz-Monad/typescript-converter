using System;
using System.IO;
using System.Linq;
using TypeScript.Syntax;
using SyntaxEditor.Model;
using TypeScript.Converter.Java;
using System.Collections.Generic;
using TypeScript.Syntax.Converter;
using TypeScript.Converter.CSharp;
using Microsoft.CodeAnalysis.CSharp;
using GrapeCity.Syntax.Converter.Source.TypeScript.Builders;

using Document = SyntaxEditor.Model.Document;
using Node = SyntaxEditor.Model.Node;
using CsharpAST = Microsoft.CodeAnalysis.SyntaxNode;
using CsharpToken = Microsoft.CodeAnalysis.SyntaxToken;
using CsharpList = Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.SyntaxNode>;
using AST = TypeScript.Syntax.Node;
using TSFile = TypeScript.Syntax.Document;
using P = System.Reflection.BindingFlags;
using System.Windows.Controls;

namespace SyntaxEditor.Utils
{
    public class Converter
    {
        private string basePath;
        private string docNamespace;
        private Lang outputLang;
        private string[] usings;
        private Project project;

        public Converter(
            string basePath,
            string docNamespace,
            Lang outputLang,
            string[] usings) : base()
        {
            this.basePath = basePath;
            this.docNamespace = docNamespace;
            this.outputLang = outputLang;
            this.usings = usings;
            var documents = BuildAst(GetDirectoryFiles(basePath));
            var project = new Project(basePath, documents);
            project.Converter = CreateConverter(project); ;
            this.project = project;
        }

        public Node ConvertToNode()
        {
            return ConvertToNode(Convert(project.Converter));
        }

        public List<Document> Convert()
        {
            return Convert(project.Converter);
        }

        private List<Document> Convert(IConverter converter)
        {
            var context = converter.Context;
            var project = context.Project;

            // normalize documents
            project.Normalize(project.Documents);

            // convert
            var result = new List<Document>();
            foreach (var doc in project.Documents)
            {
                var hook = new Hook();
                project.Converter.Hook = hook;
                converter.Convert(doc.Source);
                result.Add(ConvertToDocument(doc, ProcessTree(hook.nodes)));
            }

            return result;
        }

        private List<ConvertedNode> ProcessTree(List<ConvertedNode> nodes)
        {
            // create the tree structure
            foreach (var node in nodes)
            {
                if (node.Source.Name == null)
                {
                    node.Source = ConvertToNode(node.Source.AST);
                }
                if (node.Target.Name == null)
                {
                    node.Target = ConvertToNode(node.Target.AST);
                }
            }
            return nodes;
        }

        private Document ConvertToDocument(TSFile file, List<ConvertedNode> nodes) => new Document() {
            Name = Path.GetFileNameWithoutExtension(file.Path),
            Path = file.Path,
            Nodes = nodes,
            Source = nodes.LastOrDefault().Source,
            Target = nodes.LastOrDefault().Target
        };

        private Node ConvertToNode(List<Document> docs) => new Node()
        {
            Name = docs.GetType().Name,
            Kind = docs.GetType().Name,
            Text = "root",
            Properties = ConvertProperties(docs),
            Nodes = docs.Select(ConvertToNode).ToList(),
            AST = docs
        };

        private Node ConvertToNode(Document doc) => new Node()
        {
            Name = doc.GetType().Name,
            Kind = doc.GetType().Name,
            Text = Path.GetFileName(doc.Path),
            Properties = ConvertProperties(doc),
            Nodes = doc.Nodes.TakeLast(1).SelectMany(ConvertToNode).ToList(),
            AST = doc
        };

        private IEnumerable<Node> ConvertToNode(ConvertedNode node) => new[] { new Node()
        {
            Name = node.GetType().Name,
            Kind = node.GetType().Name,
            Text = "Source",
            Properties = ConvertProperties(node.Source),
            Nodes = Enumerable.Repeat(node.Source.AST, 1).Select(ConvertToNode).ToList(),
            AST = node.Source
        }, new Node() {
            Name = node.GetType().Name,
            Kind = node.GetType().Name,
            Text = "Target",
            Properties = ConvertProperties(node.Target),
            Nodes = Enumerable.Repeat(node.Target.AST, 1).Select(ConvertToNode).ToList(),
            AST = node.Target
        } };

        private Node ConvertToNode(object AST)
        {
            if (AST is AST ast) 
                return ConvertToNode(ast);
            else if (AST is CsharpAST csharpAst)
                return ConvertToNode(csharpAst);
            else if (AST is CsharpToken token)
                return ConvertToNode(token);
            else if (AST.IsInstanceOfGenericType(typeof(CsharpList)))
            {
                var exp = ToExpression((CsharpList arg) => ConvertToNode(arg));
                return exp.InvokeGeneric(AST);
            }
            else
                throw new InvalidOperationException();
        }

        private Node ConvertToNode(AST AST) => new Node()
        {
            Name = AST.NodeName,
            Kind = AST.Kind.ToString(),
            Text = AST.Text,
            Properties = ConvertProperties(AST),
            Nodes = AST.Children.Select(ConvertToNode).ToList(),
            AST = AST
        };

        private Node ConvertToNode(CsharpAST AST) => new Node()
        {
            Name = AST.Kind().ToString(),
            Kind = AST.Kind().ToString(),
            Text = AST.ToFullString(),
            Properties = ConvertProperties(AST),
            Nodes = AST.ChildNodes().Select(ConvertToNode).ToList(),
            AST = AST
        };

        private Node ConvertToNode<T>(Microsoft.CodeAnalysis.SyntaxList<T> AST) where T : CsharpAST => new Node()
        {
            Name = AST.GetType().Name,
            Kind = AST.GetType().Name,
            Text = AST.ToFullString(),
            Properties = ConvertProperties(AST),
            Nodes = AST.Select(ConvertToNode).ToList(),
            AST = AST
        };

        private Node ConvertToNode(CsharpToken AST) => new Node()
        {
            Name = AST.Kind().ToString(),
            Kind = AST.Kind().ToString(),
            Text = AST.ToFullString(),
            Properties = ConvertProperties(AST),
            Nodes = Enumerable.Empty<Node>().ToList(),
            AST = AST
        };

        private static List<Property> ConvertProperties(object obj)
        {
            return obj.GetType()
                .GetProperties(P.Instance | P.Public)
                .Select(prop => new Property()
                {
                    Name = prop.Name,
                    Value = SupressError(() => prop.GetValue(obj))
                })
                .ToList();
        }

        private IConverter CreateConverter(Project project)
        {
            // create context
            var context = new ConvertContext(project, this);
            switch (outputLang)
            {
                case Lang.CSharp:
                    return new CSharpConverter(context);

                case Lang.Java:
                    return new JavaConverter(context);

                default:
                    return null;
            }
        }

        private List<TSFile> BuildAst(List<string> files)
        {
            var visitor = new AnalysisVisitor();
            var documents = new List<TSFile>();
            var builder = new AbstractSyntaxTreeBuilder();
            foreach (var file in files)
            {
                documents.Add(builder.Build(file, visitor));
            }
            return documents;
        }

        private static List<string> GetDirectoryFiles(string path, SearchOption searchOption = SearchOption.AllDirectories)
        {
            var files = new List<string>();
            var fs = Directory.GetFiles(path, "*.ts.json", SearchOption.AllDirectories);
            foreach (var f in fs)
            {
                files.Add(Path.GetFullPath(f));
            }
            return files;
        }

        public enum Lang
        {
            CSharp = 1,
            Java
        }

        class ConvertContext : IConvertContext
        {
            private Project project;
            private Converter converter;

            public IProject Project => project;

            public string Namespace => converter.docNamespace;

            public bool TypeScriptType => false;

            public bool TypeScriptAdvancedType => true;

            public List<string> Usings => converter.usings.ToList();

            public List<string> QualifiedNames => new List<string>();

            public List<string> ExcludeTypes => new List<string>();

            public IOutput GetOutput(TSFile doc)
            {
                return null;
            }

            public string TrimTypeName(string typeName)
            {
                return typeName;
            }

            public ConvertContext(Project project, Converter converter)
            {
                this.project = project;
                this.converter = converter;
            }
        }

        public class ConvertedNode
        {
            public Node Source { get; set; }
            public Node Target { get; set; }
        }

        private class Hook : IConverterHook
        {
            public List<ConvertedNode> nodes { get; private set; } = new List<ConvertedNode>();

            public void Convert(AST source, object target)
            {
                nodes.Add(new ConvertedNode()
                {
                    Source = new Node() { AST = source },
                    Target = new Node() { AST = target }
                });
            }
        }

        private static T SupressError<T>(Func<T> fn)
        {
            try
            {
                return fn();
            }
            catch
            {
                return default;
            }
        }

        private static System.Linq.Expressions.Expression<Func<CsharpList, Node>> ToExpression(System.Linq.Expressions.Expression<Func<CsharpList, Node>> value)
        {
            return value;
        }
    }
}
