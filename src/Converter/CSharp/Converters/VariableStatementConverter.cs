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
    public class VariableStatementConverter : Converter
    {
        public CSharpSyntaxNode Convert(VariableStatement node)
        {
            var declaration = node.DeclarationList.ToCsNode<CSharpSyntaxNode>();
            if (declaration is VariableDeclarationSyntax var)
            {
                return SyntaxFactory
                    .LocalDeclarationStatement(var)
                    .AddModifiers(node.Modifiers.ToCsNodes<SyntaxToken>());
            }
            //modified from bindings
            return declaration; 
        }
    }
}
