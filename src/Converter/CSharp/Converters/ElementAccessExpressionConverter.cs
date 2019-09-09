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
    public class ElementAccessExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ElementAccessExpression node)
        {
            return SyntaxFactory
                .ElementAccessExpression(node.Expression.ToCsNode<ExpressionSyntax>())
                .AddArgumentListArguments(this.ToArgumentList(new List<Node>() { node.ArgumentExpression }));
        }
    }
}

