using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TypeScript.Syntax
{
    public class InterfaceDeclaration : Declaration
    {
        #region Properties
        public override NodeKind Kind
        {
            get { return NodeKind.InterfaceDeclaration; }
        }

        public List<Node> JsDoc
        {
            get;
            private set;
        }

        public List<Node> Modifiers
        {
            get;
            private set;
        }

        public Node Name
        {
            get;
            private set;
        }

        public string NameText
        {
            get
            {
                return (this.Name != null ? this.Name.Text : string.Empty);
            }
        }

        public List<Node> TypeParameters
        {
            get;
            private set;
        }

        public List<Node> HeritageClauses
        {
            get;
            private set;
        }

        public List<Node> Members
        {
            get;
            private set;
        }

        public List<Node> BaseTypes
        {
            get
            {
                List<Node> types = new List<Node>();
                foreach (HeritageClause heritage in this.HeritageClauses)
                {
                    types.AddRange(heritage.Types);
                }
                return types;
            }
        }

        public bool IsExport
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.ExportKeyword);
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.Modifiers.Exists(n => n.Kind == NodeKind.DefaultKeyword);
            }
        }
        #endregion

        public override void Init(JObject jsonObj)
        {
            base.Init(jsonObj);

            this.JsDoc = new List<Node>();
            this.Modifiers = new List<Node>();
            this.Name = null;
            this.TypeParameters = new List<Node>();
            this.HeritageClauses = new List<Node>();
            this.Members = new List<Node>();
        }

        public override void AddChild(Node childNode)
        {
            base.AddChild(childNode);

            string nodeName = childNode.NodeName;
            switch (nodeName)
            {
                case "jsDoc":
                    this.JsDoc.Add(childNode);
                    break;

                case "modifiers":
                    this.Modifiers.Add(childNode);
                    break;

                case "name":
                    this.Name = childNode;
                    break;

                case "typeParameters":
                    this.TypeParameters.Add(childNode);
                    break;

                case "heritageClauses":
                    this.HeritageClauses.Add(childNode);
                    break;

                case "members":
                    this.Members.Add(childNode);
                    break;


                default:
                    this.ProcessUnknownNode(childNode);
                    break;
            }
        }


        public Node GetMember(string name)
        {
            foreach (Node member in this.Members)
            {
                switch (member.Kind)
                {
                    case NodeKind.MethodSignature:
                        MethodSignature method = member as MethodSignature;
                        if (method.Name.Text == name)
                        {
                            return method;
                        }
                        break;

                    case NodeKind.PropertySignature:
                        PropertySignature prop = member as PropertySignature;
                        if (prop.Name.Text == name)
                        {
                            return prop;
                        }
                        break;

                    default:
                        break;
                }
            }
            return null;
        }

        public void AddMember(Node member)
        {
            member.Parent = this;
            this.Members.Add(member);
        }

        public void InsertMember(int index, Node member)
        {
            member.Parent = this;
            this.Members.Insert(index, member);
        }

        public void InsertMembers(int index, List<Node> members)
        {
            foreach (var mem in members)
            {
                mem.Parent = this;
            }
            this.Members.InsertRange(index, members);
        }

        public void RemoveMemberAt(int index)
        {
            this.Members.RemoveAt(index);
        }

    }
}

