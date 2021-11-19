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
    public class SpreadAssignmentConverter : Converter
    {
        public CSharpSyntaxNode Convert(SpreadAssignment node)
        {
            return SyntaxFactory
                .Argument(node.Expression.ToCsNode<ExpressionSyntax>())
                .WithNameColon(SyntaxFactory.NameColon("__spread__"));
        }
    }
}

