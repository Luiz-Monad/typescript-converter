using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;

namespace TypeScript.Converter.CSharp
{
    public class ArrayLiteralExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ArrayLiteralExpression node)
        {
            TypeSyntax csType = node.Type.ToCsSyntaxTree<TypeSyntax>();
            if (node.Type.Kind == NodeKind.VoidKeyword)
            {
                if (this.Context.TypeScriptType)
                {
                    csType = SyntaxFactory.GenericName("Array");
                }
                else
                {
                    csType = SyntaxFactory.GenericName("List");
                }
            }
            if (node.Elements.Count == 0)
            {
                return SyntaxFactory
                    .ObjectCreationExpression(csType)
                    .AddArgumentListArguments();
            }

            var spread = new List<Node>();
            var elements = new List<Node>();

            foreach (Node item in node.Elements)
            {
                if (item.Kind == NodeKind.SpreadElement)
                {
                    spread.Add(((SpreadElement)item).Expression);
                }
                else
                {
                    elements.Add(item);
                }
            }

            var csInitilizerExprs = SyntaxFactory
                .InitializerExpression(SyntaxKind.CollectionInitializerExpression)
                .AddExpressions(elements.ToCsSyntaxTrees<ExpressionSyntax>());
            var expr = (ExpressionSyntax)SyntaxFactory
                .ObjectCreationExpression(csType)
                .WithInitializer(csInitilizerExprs);
            if (spread.Count > 0) expr = SpreadElementConverter.CreateSpreadOperator(expr, spread);
            return expr;
        }
    }
}
