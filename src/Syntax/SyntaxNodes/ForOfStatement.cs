using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ForOfStatement : Statement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ForOfStatement; }
        }

        public Node Initializer
        {
            get;
            private set;
        }

        public Node Expression
        {
            get;
            private set;
        }

        public Node Statement
        {
            get;
            private set;
        }

        public Node Identifier
        {
            get
            {
                VariableDeclarationNode variableDeclaration = this.GetVariableDeclaration();
                if (variableDeclaration != null)
                {
                    return variableDeclaration.Name;
                }
                return null;
            }
        }

        public Node AwaitModifier { get; private set; }

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Initializer = null;
            this.Expression = null;
            this.Statement = null;
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "initializer":
                    this.Initializer = childNode;
                    break;

                case "expression":
                    this.Expression = childNode;
                    break;

                case "statement":
                    this.Statement = childNode;
                    break;

                case "awaitModifier":
                    this.AwaitModifier = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        private VariableDeclarationNode GetVariableDeclaration()
        {
            VariableDeclarationList initializer = this.Initializer as VariableDeclarationList;
            if (initializer != null && initializer.Declarations.Count > 0)
            {
                return initializer.Declarations[0] as VariableDeclarationNode;
            }
            return null;
        }
    }
}

