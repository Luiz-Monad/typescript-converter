using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class JSDocReturnTag : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.JSDocReturnTag; }
        }

        public Node AtToken
        {
            get;
            private set;
        }

        public Node TagName
        {
            get;
            private set;
        }

        public string Comment
        {
            get;
            private set;
        }

        public Node TypeExpression
        {
            get;
            private set;
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.AtToken = null;
            this.TagName = null;
            this.TypeExpression = null;

            JToken jsonComment = jsonObj["comment"];
            this.Comment = jsonComment == null ? string.Empty : jsonComment.ToObject<string>();
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "atToken":
                    this.AtToken = childNode;
                    break;

                case "tagName":
                    this.TagName = childNode;
                    break;

                case "typeExpression":
                    this.TypeExpression = childNode;
                    break;

                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}

