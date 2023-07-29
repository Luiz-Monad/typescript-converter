﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TypeScript.Syntax;
using TypeScript.Syntax.Analysis;
using TypeScript.Syntax.Converter;

namespace TypeScript.Converter.CSharp
{
    public class CSharpConverter : IConverter
    {
        private static readonly string CodeHeaderText =
           "/// This file was generated by C# converter tool" + Environment.NewLine + 
           "/// Any changes made to this file manually will be lost next time the file is regenerated." + Environment.NewLine +
            Environment.NewLine;

        #region Constructor
        public CSharpConverter(IConvertContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.Context = context;

            CSharpConverter.Current = this;
        }
        #endregion

        #region IConvertContext Interface
        /// <summary>
        /// Gets the converter context.
        /// </summary>
        public IConvertContext Context
        {
            get;
            private set;
        }

        /// <summary>
        /// Converts syntax tree node to code.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public string Convert(Node node)
        {
            return CodeHeaderText + node.ToCSharp();
        }
        #endregion

        #region IConverterHook Interface
        /// <summary>
        /// Gets the converter hook.
        /// </summary>
        public IConverterHook Hook
        {
            get;
            set;
        }
        #endregion

        #region Fields
        internal static CSharpConverter Current { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Create a converter by syntax node.
        /// </summary>
        /// <param name="node">The syntax node.</param>
        /// <returns>The node's converter.</returns>
        public NodeConverter CreateConverter(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            Type nodeType = node.GetType();
            if (ConverterTypes.ContainsKey(nodeType))
            {
                Type converterType = ConverterTypes[nodeType];
                NodeConverter converter = (NodeConverter)converterType.GetConstructor(Type.EmptyTypes).Invoke(Type.EmptyTypes);
                converter.Context = this.Context;
                return converter;
            }
            return null;
        }
        #endregion

        #region Static Members
        private static Dictionary<Type, Type> cachedConverterTypes;
        private static Dictionary<Type, Type> ConverterTypes
        {
            get
            {
                if (cachedConverterTypes != null)
                {
                    return cachedConverterTypes;
                }

                //
                cachedConverterTypes = new Dictionary<Type, Type>();
                Type baseType = typeof(NodeConverter);
                Type[] types = Assembly.GetExecutingAssembly().GetExportedTypes();

                foreach (Type type in types)
                {
                    if (!type.IsSubclassOf(baseType))
                    {
                        continue;
                    }

                    MethodInfo convererMethod = type.GetMethod("Convert");
                    if (convererMethod == null)
                    {
                        continue;
                    }

                    ParameterInfo[] parameters = convererMethod.GetParameters();
                    if (parameters != null && parameters.Length == 1)
                    {
                        Type paramType = parameters[0].ParameterType;
                        cachedConverterTypes[paramType] = type;
                    }
                }
                return cachedConverterTypes;
            }
        }
        #endregion
    }
}
