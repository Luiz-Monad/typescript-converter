using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class GreaterThanGreaterThanToken : Node
    {
        public override NodeKind Kind
        {
            get { return NodeKind.GreaterThanGreaterThanToken; }
        }

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }
    }
}
