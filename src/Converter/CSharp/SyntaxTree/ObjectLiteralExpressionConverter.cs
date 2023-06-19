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
            Node type = node.Type;
            ExpressionSyntax expr;

            var (props, spread, trivia) = ConvertProperties(node);

            if (type.Kind == NodeKind.TypeLiteral && !((TypeLiteral)type).IsIndexSignature && props.Count >= 2)
            {
                expr = SyntaxFactory.TupleExpression().AddArguments(props.ToCsSyntaxTrees<ArgumentSyntax>());
            }
            else if (type.Kind == NodeKind.AnyKeyword || type.Kind == NodeKind.VoidKeyword)
            {
                var csAnonyNewExpr = SyntaxFactory.AnonymousObjectCreationExpression();
                foreach (Node property in props)
                {
                    var (propName, valueExpr) = ConvertPropertyValue(property);

                    csAnonyNewExpr = csAnonyNewExpr.AddInitializers(SyntaxFactory.AnonymousObjectMemberDeclarator(
                        SyntaxFactory.NameEquals(NormalizeTypeName(propName)),
                        valueExpr));
                }
                expr = csAnonyNewExpr;
            }
            else
            {
                var csType = type.ToCsSyntaxTree<TypeSyntax>();
                var csObjLiteral = SyntaxFactory.ObjectCreationExpression(csType)
                    .AddArgumentListArguments();
                var initItemExprs = new List<ExpressionSyntax>();
                foreach (Node property in props)
                {
                    var (propName, valueExpr) = ConvertPropertyValue(property);

                    var csNameExpression = SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(NormalizeTypeName(propName)));
                    var itemInitExpr = SyntaxFactory
                        .InitializerExpression(SyntaxKind.ComplexElementInitializerExpression)
                        .AddExpressions(csNameExpression, valueExpr);

                    initItemExprs.Add(itemInitExpr);
                }
                if (initItemExprs.Count > 0)
                {
                    expr = csObjLiteral.WithInitializer(SyntaxFactory.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SyntaxFactory.SeparatedList(initItemExprs)));
                }
                else
                {
                    expr = csObjLiteral;
                }
            }
            if (spread.Count > 0) expr = SpreadElementConverter.CreateSpreadOperator(expr, spread);
            if (trivia.Count > 0) expr = expr.WithTrailingComment(String.Join(' ', trivia));
            return expr;
        }

        private (Node propName, ExpressionSyntax expr) ConvertPropertyValue(Node property)
        {
            Node propName = null;
            ExpressionSyntax valueExpr = null;

            switch (property.Kind)
            {
                case NodeKind.PropertyAssignment:
                    var prop = (PropertyAssignment)property;
                    propName = prop.Name;
                    valueExpr = prop.Initializer.ToCsSyntaxTree<ExpressionSyntax>();
                    break;

                case NodeKind.ShorthandPropertyAssignment:
                    var shortProp = (ShorthandPropertyAssignment)property;
                    propName = shortProp.Name;
                    valueExpr = SyntaxFactory.ParseName(NormalizeTypeName(propName));
                    break;

                default:
                    throw new InvalidOperationException();
            }

            return (propName, valueExpr);
        }

        private (List<Node> props, List<Node> spread, List<String> trivia) ConvertProperties(ObjectLiteralExpression node)
        {
            var spread = new List<Node>();
            var props = new List<Node>();
            var trivia = new List<String>();

            foreach (Node property in node.Properties)
            {
                switch (property.Kind)
                {
                    case NodeKind.PropertyAssignment:
                    case NodeKind.ShorthandPropertyAssignment:
                        props.Add(property);
                        break;

                    case NodeKind.SpreadAssignment:
                        spread.Add(((SpreadAssignment)property).Expression);
                        break;

                    default:
                        trivia.Add(property.Text);
                        break;
                }
            }

            return (props, spread, trivia);
        }
    }
}
