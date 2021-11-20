using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class SpreadAssignment : SpreadElement
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.SpreadAssignment; }
        }
        #endregion


        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.Expression = null;
        }

    }
}

