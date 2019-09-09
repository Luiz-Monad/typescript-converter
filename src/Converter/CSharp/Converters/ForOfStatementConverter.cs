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
    public class ForOfStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(ForOfStatement node)
        {
            return SyntaxFactory.ForEachStatement(
                SyntaxFactory.IdentifierName("var"),
                node.Identifier.Text,
                node.Expression.ToCsNode<ExpressionSyntax>(),
                node.Statement.ToCsNode<StatementSyntax>());
        }
    }
}

