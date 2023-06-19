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
    public class AwaitExpressionConverter : NodeConverter
    {
		public CSharpSyntaxNode Convert(AwaitExpression node)
        {
            //TODO: implement custom types instead of using the base type.
            if (node.Literal.Kind == NodeKind.StringLiteral)
            {
                return NodeHelper.CreateNode(NodeKind.StringKeyword).ToCsNode<TypeSyntax>()
                    .WithTrailingComment(node.Text);
            }
            else if (node.Literal.Kind == NodeKind.NumericLiteral)
            {
                return NodeHelper.CreateNode(NodeKind.NumberKeyword).ToCsNode<TypeSyntax>()
                    .WithTrailingComment(node.Text);
            }
            else if (node.Literal.Kind == NodeKind.TrueKeyword || node.Literal.Kind == NodeKind.FalseKeyword)
            {
                return NodeHelper.CreateNode(NodeKind.BooleanKeyword).ToCsNode<TypeSyntax>()
                    .WithTrailingComment(node.Text);
            }

            //TODO: NOT SUPPORT

            return SyntaxFactory.AwaitExpression(node.Expression.ToCsSyntaxTree<ExpressionSyntax>());

        }
    }
}

