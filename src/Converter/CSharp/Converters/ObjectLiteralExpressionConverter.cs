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
            Node type = node.Type;

            var spread = new List<SpreadElement>();
            var properties = new List<PropertyAssignment>();
            var trivia = new StringBuilder();

            foreach (Node property in node.Properties)
            {
                if (property.Kind == NodeKind.PropertyAssignment || property.Kind == NodeKind.ShorthandPropertyAssignment)
                {
                    var prop = property as PropertyAssignment;
                    properties.Add(prop);
                }
                else if (property.Kind == NodeKind.SpreadAssignment)
                {
                    var prop = property as SpreadAssignment;
                    spread.Add(prop);
                }
                else
                {
                    trivia.Append(node.Text);
                    trivia.Append(' ');
                }
            }

            if (type.Kind == NodeKind.TypeLiteral && properties.Count >= 2 &&
                (type as TypeLiteral).Members.Count > 0 && (type as TypeLiteral).Members[0].Kind != NodeKind.IndexSignature)
            {
                ExpressionSyntax expr = SyntaxFactory.TupleExpression().AddArguments(properties.ToCsNodes<ArgumentSyntax>());
                if (spread.Count > 0) expr = SpreadElementConverter.CreateSpreadOperator(expr, spread);
                if (trivia.Length > 0) expr = expr.WithTrailingComment(trivia.ToString());
                return expr;
            }
            else if (type.Kind == NodeKind.AnyKeyword)
            {
                var csAnonyNewExpr = SyntaxFactory.AnonymousObjectCreationExpression();
                foreach (PropertyAssignment prop in properties)
                {
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
                ExpressionSyntax expr = csAnonyNewExpr;
                if (spread.Count > 0) expr = SpreadElementConverter.CreateSpreadOperator(expr, spread);
                if (trivia.Length > 0) expr = expr.WithTrailingComment(trivia.ToString());
                return expr;
            }
            else
            {
                var csObjLiteral = SyntaxFactory.ObjectCreationExpression(type.ToCsNode<TypeSyntax>()).AddArgumentListArguments();
                List<ExpressionSyntax> initItemExprs = new List<ExpressionSyntax>();
                foreach (PropertyAssignment prop in properties)
                {
                    var csNameExpression =
                        SyntaxFactory.IdentifierName(prop.Name.Text);
                    var itemInitExpr = SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        csNameExpression, 
                        prop.Initializer.ToCsNode<ExpressionSyntax>());

                    initItemExprs.Add(itemInitExpr);
                }
                if (initItemExprs.Count > 0)
                {
                    csObjLiteral = csObjLiteral.WithInitializer(SyntaxFactory.InitializerExpression(
                        SyntaxKind.CollectionInitializerExpression,
                        SyntaxFactory.SeparatedList(initItemExprs)));
                }
                ExpressionSyntax expr = csObjLiteral;
                if (spread.Count > 0) expr = SpreadElementConverter.CreateSpreadOperator(expr, spread);
                if (trivia.Length > 0) expr = expr.WithTrailingComment(trivia.ToString());
                return expr;
            }
        }

    }
}
