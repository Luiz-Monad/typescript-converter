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
    public class ObjectLiteralExpressionConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ObjectLiteralExpression node)
        {
            List<Node> properties = node.Properties;
            Node type = node.Type;

            if (type.Kind == NodeKind.TypeLiteral && !((TypeLiteral)type).IsIndexSignature && properties.Count >= 2)
            {
                return SyntaxFactory.TupleExpression().AddArguments(properties.ToCsSyntaxTrees<ArgumentSyntax>());
            }
            else if (type.Kind == NodeKind.AnyKeyword)
            {
                AnonymousObjectCreationExpressionSyntax csAnonyNewExpr = SyntaxFactory.AnonymousObjectCreationExpression();
                foreach (Node property in node.Properties)
                {
                    string propName = null;
                    Node initValue = null;
                    ExpressionSyntax valueExpr = null;

                    switch (property.Kind)
                    {
                        case NodeKind.PropertyAssignment:
                            var prop = (PropertyAssignment)property;
                            propName = prop.Name.Text;
                            initValue = prop.Initializer;
                            valueExpr = initValue.ToCsSyntaxTree<ExpressionSyntax>();
                            break;

                        case NodeKind.ShorthandPropertyAssignment:
                            var shortProp = (ShorthandPropertyAssignment)property;
                            propName = shortProp.Name.Text;
                            initValue = type; 
                            valueExpr = SyntaxFactory.ParseName(propName);
                            break;

                        case NodeKind.SpreadAssignment:
                            //TODO: spread
                            continue;

                        default:
                            continue;
                    }

                    if (type.Kind == NodeKind.TypeLiteral && initValue.Kind == NodeKind.NullKeyword)
                    {
                        Node memType = TypeHelper.GetTypeLiteralMemberType((TypeLiteral)type, propName);
                        if (memType != null)
                        {
                            valueExpr = SyntaxFactory.CastExpression(memType.ToCsSyntaxTree<TypeSyntax>(), valueExpr);
                        }
                    }

                    csAnonyNewExpr = csAnonyNewExpr.AddInitializers(SyntaxFactory.AnonymousObjectMemberDeclarator(
                        SyntaxFactory.NameEquals(propName),
                        valueExpr));
                }
                return csAnonyNewExpr;
            }
            else
            {
                ObjectCreationExpressionSyntax csObjLiteral = SyntaxFactory.ObjectCreationExpression(type.ToCsSyntaxTree<TypeSyntax>()).AddArgumentListArguments();
                List<ExpressionSyntax> initItemExprs = new List<ExpressionSyntax>();
                foreach (Node property in node.Properties)
                {
                    string propName = null;
                    ExpressionSyntax valueExpr = null;

                    switch (property.Kind)
                    {
                        case NodeKind.PropertyAssignment:
                            var prop = (PropertyAssignment)property;
                            propName = prop.Name.Text;
                            valueExpr = prop.Initializer.ToCsSyntaxTree<ExpressionSyntax>();
                            break;
                            
                        case NodeKind.ShorthandPropertyAssignment:
                            var shortProp = (ShorthandPropertyAssignment)property;
                            propName = shortProp.Name.Text;
                            valueExpr = SyntaxFactory.ParseName(propName);
                            break;

                        case NodeKind.SpreadAssignment:
                            //TODO: spread
                            continue;

                        default:
                            continue;                            
                    }

                    ExpressionSyntax csNameExpression = SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(propName));
                    InitializerExpressionSyntax itemInitExpr = SyntaxFactory
                        .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                        .AddExpressions(csNameExpression, valueExpr);

                    initItemExprs.Add(itemInitExpr);
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
