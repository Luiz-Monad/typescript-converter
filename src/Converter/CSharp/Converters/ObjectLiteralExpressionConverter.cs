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
    public class ObjectLiteralExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ObjectLiteralExpression node)
        {
            List<Node> properties = node.Properties;
            Node type = node.Type;

            if (type.Kind == NodeKind.TypeLiteral && properties.Count >= 2 &&
                (type as TypeLiteral).Members.Count > 0 && (type as TypeLiteral).Members[0].Kind != NodeKind.IndexSignature)
            {
                return SyntaxFactory.TupleExpression().AddArguments(properties.ToCsNodes<ArgumentSyntax>());
            }
            else if (type.Kind == NodeKind.AnyKeyword)
            {
                AnonymousObjectCreationExpressionSyntax csAnonyNewExpr = SyntaxFactory.AnonymousObjectCreationExpression();
                var spread = new List<Node>();
                foreach (Node property in node.Properties)
                {
                    if (property.Kind == NodeKind.PropertyAssignment || property.Kind == NodeKind.ShorthandPropertyAssignment)
                    {
                        var prop = property as PropertyAssignment;
                        string propName = prop.Name.Text;
                        Node initValue = prop.Initializer;
                        ExpressionSyntax valueExpr = initValue.ToCsNode<ExpressionSyntax>();

                        if (type.Kind == NodeKind.TypeLiteral && initValue.Kind == NodeKind.NullKeyword)
                        {
                            Node memType = TypeHelper.GetTypeLiteralMemberType(type as TypeLiteral, propName);
                            if (memType != null)
                            {
                                valueExpr = SyntaxFactory.CastExpression(memType.ToCsNode<TypeSyntax>(), valueExpr);
                            }
                        }

                        csAnonyNewExpr = csAnonyNewExpr.AddInitializers(SyntaxFactory.AnonymousObjectMemberDeclarator(
                            SyntaxFactory.NameEquals(propName),
                            valueExpr));

                    }
                    else if (property.Kind == NodeKind.SpreadAssignment)
                    {
                        var prop = property as SpreadAssignment;
                        spread.Add(prop.Expression);
                    }
                    else
                    {
                        spread.Add(NodeHelper.CreateNode(NodeKind.WhitespaceTrivia, this.CommentText(node.Text)));
                    }
                }
                if (spread.Count > 0)
                {
                    var arguments = new List<ArgumentSyntax>();
                    arguments.Add(SyntaxFactory.Argument(csAnonyNewExpr));
                    arguments.AddRange(this.ToArgumentList(spread));
                    return SyntaxFactory
                       .InvocationExpression(SyntaxFactory.ParseExpression("__spread__"))
                       .AddArgumentListArguments(arguments.ToArray());
                }
                return csAnonyNewExpr;
            }
            else
            {
                ObjectCreationExpressionSyntax csObjLiteral = SyntaxFactory.ObjectCreationExpression(type.ToCsNode<TypeSyntax>()).AddArgumentListArguments();
                List<ExpressionSyntax> initItemExprs = new List<ExpressionSyntax>();
                foreach (Node property in node.Properties)
                {
                    if (property.Kind == NodeKind.PropertyAssignment || property.Kind == NodeKind.ShorthandPropertyAssignment)
                    {
                        var prop = property as PropertyAssignment;
                        ExpressionSyntax csNameExpression = SyntaxFactory.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal(prop.Name.Text));
                        InitializerExpressionSyntax itemInitExpr = SyntaxFactory
                            .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                            .AddExpressions(csNameExpression, prop.Initializer.ToCsNode<ExpressionSyntax>());

                        initItemExprs.Add(itemInitExpr);
                    }
                    else if (property.Kind == NodeKind.SpreadAssignment)
                    {
                        var prop = property as SpreadAssignment;
                        ExpressionSyntax csNameExpression = SyntaxFactory.LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal("__spread__"));
                        InitializerExpressionSyntax itemInitExpr = SyntaxFactory
                            .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                            .AddExpressions(csNameExpression, prop.Expression.ToCsNode<ExpressionSyntax>());

                        initItemExprs.Add(itemInitExpr);
                    }
                    else
                    {
                        initItemExprs.Add(SyntaxFactory.ParseExpression(this.CommentText(node.Text)));
                    }
                }
                if (initItemExprs.Count > 0)
                {
                    return csObjLiteral.WithInitializer(SyntaxFactory.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SyntaxFactory.SeparatedList(initItemExprs)));
                }
                return csObjLiteral;
            }
        }

    }
}

