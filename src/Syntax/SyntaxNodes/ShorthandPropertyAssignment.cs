using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class ShorthandPropertyAssignment : PropertyAssignment
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.ShorthandPropertyAssignment; }
        }

        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            base.Initializer = base.Name;
        }

    }
}

