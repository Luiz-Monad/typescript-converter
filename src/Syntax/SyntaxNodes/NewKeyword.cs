﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class NewKeyword : Node
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.NewKeyword; }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
        }

        public override void AddNode(Node childNode)
        {
            base.AddNode(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }

        internal override string GetText()
        {
            return "new";
        }
    }
}

