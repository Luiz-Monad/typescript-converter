using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;
using System.Linq;

namespace TypeScript.Converter.CSharp
{
    public class SpreadElementConverter : Converter
    {
        public CSharpSyntaxNode Convert(SpreadElement node)
        {
            if (node.Parent != null && node.Parent.Kind == NodeKind.CallExpression)
            {
                return node.Expression.ToCsNode<ExpressionSyntax>();
            }

            // this shouldn't be called anymore as (Array|Object)LiteralExpressionConverter takes care of it.
            return SyntaxFactory
                .Argument(node.Expression.ToCsNode<ExpressionSyntax>())
                .WithNameColon(SyntaxFactory.NameColon("__spread__"));
        }
        internal static ExpressionSyntax CreateSpreadOperator(ExpressionSyntax expr, IList<SpreadElement> spread)
        {
            var arguments = new List<ArgumentSyntax>();
            arguments.AddRange(spread.Select(s => SyntaxFactory.Argument(s.Expression.ToCsNode<ExpressionSyntax>())));

            // static call
            //arguments.Insert(0, SyntaxFactory.Argument(expr));
            //return SyntaxFactory
            //    .InvocationExpression(SyntaxFactory.ParseExpression("Object.Spread"))
            //    .AddArgumentListArguments(arguments.ToArray());

            // extension method
            return
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.ParenthesizedExpression(expr),
                        SyntaxFactory.IdentifierName("Spread")))
                    .WithArgumentList(SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList(arguments.ToArray())));
        }

    }
}

