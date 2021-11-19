using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypeScript.Syntax;

namespace TypeScript.Converter.CSharp
{
    public class IntersectionTypeConverter : Converter
    {
        public CSharpSyntaxNode Convert(IntersectionType node)
        {
            return SyntaxFactory.IdentifierName("dynamic");
        }
    }
}
