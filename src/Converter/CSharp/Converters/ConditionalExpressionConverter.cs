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
    public class ConditionalExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ConditionalExpression node)
        {
            return SyntaxFactory.ConditionalExpression(
                 node.Condition.ToCsNode<ExpressionSyntax>(),
                 node.WhenTrue.ToCsNode<ExpressionSyntax>(),
                 node.WhenFalse.ToCsNode<ExpressionSyntax>());
        }
    }
}

