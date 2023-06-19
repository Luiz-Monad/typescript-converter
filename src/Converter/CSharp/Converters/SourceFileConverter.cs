using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;

namespace TypeScript.Converter.CSharp
{
    public class SourceFileConverter : Converter
    {
        public CSharpSyntaxNode Convert(SourceFile sourceFile)
        {
            CompilationUnitSyntax csCompilationUnit = SyntaxFactory.CompilationUnit();
            foreach (string us in this.Context.Config.Usings)
            {
                csCompilationUnit = csCompilationUnit.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(us)));
            }

            csCompilationUnit = csCompilationUnit.AddMembers(sourceFile.ModuleDeclarations.ToCsNodes<MemberDeclarationSyntax>());

            foreach (Node import in sourceFile.ImportDeclarations)
            {
                foreach (UsingDirectiveSyntax usingSyntax in import.ToCsNode<SyntaxList<UsingDirectiveSyntax>>())
                {
                    csCompilationUnit = csCompilationUnit.AddUsings(usingSyntax);
                }
            }

            var statics = new List<MemberDeclarationSyntax>();
            var staticStatements = sourceFile.Statements
                .Except(sourceFile.TypeAliases)
                .Except(sourceFile.TypeDefinitions)
                .Except(sourceFile.ImportDeclarations)
                .Except(sourceFile.ModuleDeclarations)
                .ToList(); //const intializers
            if (staticStatements.Count > 0)
            {
                var className = Path.GetFileNameWithoutExtension(sourceFile.FileName);
                statics.Add(CreateStaticClass(className, staticStatements));
            }

            var typeAliases = sourceFile.TypeAliases;
            var typeDefinitions = this.FilterTypes(sourceFile.TypeDefinitions);
            if (typeAliases.Count > 0 || typeDefinitions.Count > 0)
            {
                var ns = sourceFile.Document.GetPackageName();
                var usings = typeAliases.ToCsNodes<UsingDirectiveSyntax>();
                var members = typeDefinitions.ToCsNodes<MemberDeclarationSyntax>().Union(statics).ToArray();
                if (!string.IsNullOrEmpty(ns))
                {
                    NamespaceDeclarationSyntax nsSyntaxNode = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns));
                    nsSyntaxNode = nsSyntaxNode
                        .AddUsings(usings)
                        .AddMembers(members);
                    csCompilationUnit = csCompilationUnit
                        .AddMembers(nsSyntaxNode);
                }
                else
                {
                    csCompilationUnit = csCompilationUnit
                        .AddUsings(usings)
                        .AddMembers(members);
                }
            }

            return csCompilationUnit;
        }

        private ClassDeclarationSyntax CreateStaticClass(string className, List<Node> members)
        {
            var csMembers = members.ToCsNodes<StatementSyntax>();

            foreach (var memb in csMembers)
            {
                if (memb.DescendantNodesAndSelf VariableDeclarationSyntax)
            }

            var block = SyntaxFactory
                .Block(csMembers);

            var csCtor = SyntaxFactory
                .ConstructorDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .AddParameterListParameters()
                .WithBody(block);

            var csClass = SyntaxFactory
                .ClassDeclaration(className)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .AddMembers(members.ToCsNodes<MemberDeclarationSyntax>())
                .AddMembers(csCtor);

            return csClass;
        }

    }
}

