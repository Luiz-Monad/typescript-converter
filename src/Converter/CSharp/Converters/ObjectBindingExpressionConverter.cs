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
    public class ObjectBindingExpressionConverter : Converter
    {
        public CSharpSyntaxNode Convert(ObjectBindingPattern node)
        {
            return CreateBinding(node, node.Elements);
        }

        internal static CSharpSyntaxNode CreateBinding(Node node, List<Node> elements)
        {
            var initializer = (node.Parent as VariableDeclarationNode)?.Initializer;
            var vars = new List<VariableDesignationSyntax>();
            var values = new Dictionary<string, Node>();
            var ix = 0;
            foreach (var e in elements)
            {
                string name = $"_p{ix++}";
                Node value = null;
                if (e is BindingElement bind)
                {
                    if (bind.Name.Kind == NodeKind.Identifier)
                    {
                        name = bind.PropertyName?.Text ?? bind.Name.Text;
                    }
                    else
                    {
                        name = bind.PropertyName?.Text ?? name;
                        value = bind.Name;
                    }
                }
                else
                {
                    value = e;
                }
                vars.Add(SyntaxFactory.SingleVariableDesignation(
                    SyntaxFactory.Identifier(name)));
                values.Add(name, value);
            }
            VariableDesignationSyntax designation =
                SyntaxFactory.ParenthesizedVariableDesignation(
                    SyntaxFactory.SeparatedList(vars));
            if (vars.Count == 1)
            {
                designation = vars[0];
            }
            if (initializer == null)
            {
                return CreateClosure(
                    c => CreateApplyTuple(c, values));
            }
            return SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.DeclarationExpression(
                    SyntaxFactory.IdentifierName("var"),
                    designation),
                CreateApplyTuple(
                    initializer.ToCsNode<ExpressionSyntax>(),
                    values));
        }

        private static ExpressionSyntax CreateApplyTuple(ExpressionSyntax expr, Dictionary<string, Node> values)
        {
            // Apply avoids needing to implement Deconstruct extension methods, we use a tuple.
            // expr.Apply(_o => (_o.property, _o.property2))
            var lambdaVar = SyntaxFactory.IdentifierName("_o");
            var props = values.Select(v =>
                v.Value == null ?
                    SyntaxFactory.Argument(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            lambdaVar,
                            SyntaxFactory.IdentifierName(v.Key))) :
                    SyntaxFactory.Argument(v.Value.ToCsNode<ExpressionSyntax>())
                        .WithNameColon(SyntaxFactory.NameColon(
                            SyntaxFactory.IdentifierName(v.Key)))
            ).ToArray();
            ExpressionSyntax tuple = props.Length == 1 ? props[0].Expression :
                SyntaxFactory.TupleExpression(
                    SyntaxFactory.SeparatedList(props));
            var lambda = SyntaxFactory.SimpleLambdaExpression(
                SyntaxFactory.Parameter(lambdaVar.Identifier),
                tuple);
            var method = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.ParenthesizedExpression(expr),
                SyntaxFactory.IdentifierName("Apply"));
            return SyntaxFactory.InvocationExpression(method, SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(lambda))));
            // TODO: bubble up await expression
        }

        private static CSharpSyntaxNode CreateClosure(Func<ExpressionSyntax, ExpressionSyntax> func)
        {
            // _c => closure(_c)
            var lambdaVar = SyntaxFactory.IdentifierName("_c");
            var lambda = SyntaxFactory.SimpleLambdaExpression(
                SyntaxFactory.Parameter(lambdaVar.Identifier),
                func(lambdaVar));
            return SyntaxFactory.InvocationExpression(lambda, SyntaxFactory.ArgumentList(
                SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(lambda))));
        }

    }
}
