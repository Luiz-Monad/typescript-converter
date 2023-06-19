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
    public class ArrayBindingPatternConverter : NodeConverter
    {
        public CSharpSyntaxNode Convert(ArrayBindingPattern node)
        {
            // this shouldn't be called anymore as (Array|Object)LiteralExpressionConverter takes care of it.
            return SyntaxFactory.ParseExpression(this.CommentText(node.Text));
        }

    }
}
