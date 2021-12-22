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
    public class VariableDeclarationListConverter : Converter
    {
        public CSharpSyntaxNode Convert(VariableDeclarationList node)
        {
            bool isVar = false;
            Node type = node.Type;
            if (type.Kind == NodeKind.AnyKeyword && node.Declarations.Count > 0)
            {
                VariableDeclarationNode variableNode = node.Declarations[0] as VariableDeclarationNode;
                if (variableNode.Initializer != null && variableNode.Initializer.Kind != NodeKind.NullKeyword)
                {
                    isVar = true;
                }
            }

            var decls = node.Declarations.ToCsNodes<CSharpSyntaxNode>();

            // check binding rewrites
            if (!decls.All(d => d is VariableDeclaratorSyntax))
            {
                return decls[0];
            }

            TypeSyntax csType = isVar ? SyntaxFactory.IdentifierName("var") : node.Type.ToCsNode<TypeSyntax>();
            return SyntaxFactory
                .VariableDeclaration(csType)
                .AddVariables(decls.OfType<VariableDeclaratorSyntax>().ToArray());
        }
    }
}
