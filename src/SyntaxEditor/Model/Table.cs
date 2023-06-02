using System.Collections.Generic;
using System.Drawing;

namespace SyntaxEditor.Model
{
    public class Table
    {

        public static readonly Dictionary<string, string> PropertyCategory =
            new()
            {
                { "Kind", "General" },
                { "NodeName", "General" },
                { "Name", "General" },
                { "NameText", "General" },
                { "Pos", "General" },
                { "End", "General" },
                { "Text", "General" },

                { "Children", "Base" },
                { "Document", "Base" },
                { "Flags", "Base" },
                { "Parent", "Base" },
                { "Path", "Base" },
                { "RelativePath", "Base" },
                { "Project", "Base" },
                { "Root", "Base" },

                { "Identifier", "Attributes" },
                { "Base", "Attributes" },
                { "Type", "Attributes" },
                { "TypeName", "Attributes" },
                { "KeyType", "Attributes" },
                { "ElementType", "Attributes" },
                { "Modifiers", "Attributes" },
                { "ExprName", "Attributes" },
                { "PropertyName", "Attributes" },
                { "ParameterName", "Attributes" },
                { "DefinitionName", "Attributes" },
                { "Multiline", "Attributes" },
                { "IsParams", "Attributes" },
                { "IsDefault", "Attributes" },
                { "IsExport", "Attributes" },
                { "IsAbstract", "Attributes" },
                { "IsPrivate", "Attributes" },
                { "IsPublic", "Attributes" },
                { "IsStatic", "Attributes" },
                { "IsGenericType", "Attributes" },
                { "IsOptional", "Attributes" },
                { "IsVariable", "Attributes" },
                { "IsReadOnly", "Attributes" },
                { "HasNullType", "Attributes" },
                { "AwaitModifier", "Attributes" },
                { "Token", "Attributes" },
                { "OperatorToken", "Attributes" },
                { "Literal", "Attributes" },
                { "IgnoreType", "Attributes" },
                { "Constraint", "Attributes" },

                { "As", "Expression" },
                { "Expression", "Expression" },
                { "ArgumentExpression", "Expression" },
                { "Left", "Expression" },
                { "Right", "Expression" },
                { "Initializer", "Expression" },
                { "ExportClause", "Expression" },
                { "CatchClause", "Expression" },
                { "Body", "Expression" },
                { "Block", "Expression" },
                { "CaseBlock", "Expression" },
                { "FinallyBlock", "Expression" },
                { "TryBlock", "Expression" },
                { "Statement", "Expression" },
                { "ElseStatement", "Expression" },
                { "ThenStatement", "Expression" },
                { "Operand", "Expression" },
                { "Operator", "Expression" },
                { "VariableDeclaration", "Expression" },
                { "Condition", "Expression" },
                { "GetAccessor", "Expression" },
                { "SetAccessor", "Expression" },

                { "Elements", "Container" },
                { "Types", "Container" },
                { "BaseTypes", "Container" },
                { "ElementTypes", "Container" },
                { "Parameters", "Container" },
                { "Arguments", "Container" },
                { "TypeAliases", "Container" },
                { "TypeDefinitions", "Container" },
                { "TypeArguments", "Container" },
                { "TypeParameters", "Container" },
                { "Statements", "Container" },
                { "Clauses", "Container" },
                { "HeritageClauses", "Container" },
                { "Members", "Container" },
                { "Initializers", "Container" },
                { "Properties", "Container" },
                { "Decorators", "Container" },
                { "DeclarationList", "Container" },

            };

        private static readonly Color Keyword = Color.CornflowerBlue;
        private static readonly Color Comment = Color.DarkSeaGreen;
        private static readonly Color Literal = Color.DodgerBlue;
        private static readonly Color Strings = Color.ForestGreen;
        private static readonly Color Identif = Color.DimGray;
        private static readonly Color Errored = Color.Firebrick;

        public static readonly Dictionary<string, Color> KindColor =
            new()
            {

                // Comments
                { "SingleLineCommentTrivia", Comment },
                { "MultiLineCommentTrivia", Comment },
                { "ShebangTrivia", Comment },
                { "ConflictMarkerTrivia", Comment },

                // Literals
                { "NumericLiteral", Literal },
                { "BigIntLiteral", Literal },
                { "StringLiteral", Literal },
                { "JsxText", Comment },
                { "RegularExpressionLiteral", Strings },
                { "NoSubstitutionTemplateLiteral", Strings },

                // Pseudo-literals
                { "TemplateHead", Strings },
                { "TemplateMiddle", Strings },
                { "TemplateTail", Strings },

                // Identifiers and PrivateIdentifiers
                { "Identifier", Identif },
                { "PrivateIdentifier", Identif },

                // Reserved words
                { "BreakKeyword", Keyword },
                { "CaseKeyword", Keyword },
                { "CatchKeyword", Keyword },
                { "ClassKeyword", Keyword },
                { "ConstKeyword", Keyword },
                { "ContinueKeyword", Keyword },
                { "DebuggerKeyword", Keyword },
                { "DefaultKeyword", Keyword },
                { "DeleteKeyword", Keyword },
                { "DoKeyword", Keyword },
                { "ElseKeyword", Keyword },
                { "EnumKeyword", Keyword },
                { "ExportKeyword", Keyword },
                { "ExtendsKeyword", Keyword },
                { "FalseKeyword", Keyword },
                { "FinallyKeyword", Keyword },
                { "ForKeyword", Keyword },
                { "FunctionKeyword", Keyword },
                { "IfKeyword", Keyword },
                { "ImportKeyword", Keyword },
                { "InKeyword", Keyword },
                { "InstanceOfKeyword", Keyword },
                { "NewKeyword", Keyword },
                { "NullKeyword", Keyword },
                { "ReturnKeyword", Keyword },
                { "SuperKeyword", Keyword },
                { "SwitchKeyword", Keyword },
                { "ThisKeyword", Keyword },
                { "ThrowKeyword", Keyword },
                { "TrueKeyword", Keyword },
                { "TryKeyword", Keyword },
                { "TypeOfKeyword", Keyword },
                { "VarKeyword", Keyword },
                { "VoidKeyword", Keyword },
                { "WhileKeyword", Keyword },
                { "WithKeyword", Keyword },
                { "ImplementsKeyword", Keyword },
                { "InterfaceKeyword", Keyword },
                { "LetKeyword", Keyword },
                { "PackageKeyword", Keyword },
                { "PrivateKeyword", Keyword },
                { "ProtectedKeyword", Keyword },
                { "PublicKeyword", Keyword },
                { "StaticKeyword", Keyword },
                { "YieldKeyword", Keyword },
                { "AbstractKeyword", Keyword },
                { "AsKeyword", Keyword },
                { "AssertsKeyword", Keyword },
                { "AnyKeyword", Keyword },
                { "AsyncKeyword", Keyword },
                { "AwaitKeyword", Keyword },
                { "BooleanKeyword", Keyword },
                { "ConstructorKeyword", Keyword },
                { "DeclareKeyword", Keyword },
                { "GetKeyword", Keyword },
                { "InferKeyword", Keyword },
                { "IsKeyword", Keyword },
                { "KeyOfKeyword", Keyword },
                { "ModuleKeyword", Keyword },
                { "NamespaceKeyword", Keyword },
                { "NeverKeyword", Keyword },
                { "ReadonlyKeyword", Keyword },
                { "RequireKeyword", Keyword },
                { "NumberKeyword", Keyword },
                { "ObjectKeyword", Keyword },
                { "SetKeyword", Keyword },
                { "StringKeyword", Keyword },
                { "SymbolKeyword", Keyword },
                { "TypeKeyword", Keyword },
                { "UndefinedKeyword", Keyword },
                { "UniqueKeyword", Keyword },
                { "UnknownKeyword", Keyword },
                { "FromKeyword", Keyword },
                { "GlobalKeyword", Keyword },
                { "BigIntKeyword", Keyword },
                { "OfKeyword" , Keyword },

                // Names
                { "QualifiedName", Identif },
                { "ComputedPropertyName", Identif },

                // Signature elements
                { "TypeParameter", Identif },
                { "Parameter", Identif },
                { "Decorator", Identif },

                // Type
                { "TypePredicate", Identif },
                { "TypeReference", Identif },
                { "FunctionType", Identif },
                { "ConstructorType", Identif },
                { "TypeQuery", Identif },
                { "TypeLiteral", Identif },
                { "ArrayType", Identif },
                { "TupleType", Identif },
                { "OptionalType", Identif },
                { "RestType", Identif },
                { "UnionType", Identif },
                { "IntersectionType", Identif },
                { "ConditionalType", Identif },
                { "InferType", Identif },
                { "ParenthesizedType", Identif },
                { "ThisType", Identif },
                { "TypeOperator", Identif },
                { "IndexedAccessType", Identif },
                { "MappedType", Identif },
                { "LiteralType", Identif },
                { "ImportType", Identif },

                // JSX
                { "JsxElement", Comment },
                { "JsxSelfClosingElement", Comment },
                { "JsxOpeningElement", Comment },
                { "JsxClosingElement", Comment },
                { "JsxFragment", Comment },
                { "JsxOpeningFragment", Comment },
                { "JsxClosingFragment", Comment },
                { "JsxAttribute", Comment },
                { "JsxAttributes", Comment },
                { "JsxSpreadAttribute", Comment },
                { "JsxExpression", Comment },

                // Unparsed
                { "UnparsedPrologue", Errored },
                { "UnparsedPrepend", Errored },
                { "UnparsedText", Errored },
                { "UnparsedInternalText", Errored },
                { "UnparsedSyntheticReference", Errored },

                // Enum value count
                { "Count",  Identif },

                /* Follows are added by the tool. Others are from TypeScript*/
                { "GetSetAccessor", Keyword },
                { "OverrideKeyword", Keyword },
                { "VirtualKeyword", Keyword },

                /* @internal */
                { "FirstContextualKeyword", Keyword },
                { "LastContextualKeyword", Keyword },

            };

    }
}
