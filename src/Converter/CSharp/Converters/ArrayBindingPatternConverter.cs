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
    public class ArrayBindingPatternConverter : Converter
    {
        public CSharpSyntaxNode Convert(ArrayBindingPattern node)
        {
            return ObjectBindingExpressionConverter.CreateBinding(node, node.Elements);
        }

    }
}

