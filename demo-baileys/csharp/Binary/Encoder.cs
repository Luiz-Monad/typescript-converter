/// This file was generated by C# converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

using System.Linq;
using TypeScript.CSharp;

namespace Bailey
{
    public class Encoder
    {
        public Encoder()
        {
            this.data = new List<double>();
        }

        public List<double> data { get; set; }

        public void pushByte(double value)
        {
            this.data.push(value & 255);
        }

        public void pushInt(double value, double n, bool littleEndian = false)
        {
            for (var i = 0; i < n; i++)
            {
                var curShift = littleEndian ? i : n - 1 - i;
                this.data.push((value >> (curShift * 8)) & 255);
            }
        }

        public void pushInt20(double value)
        {
            this.pushBytes(new OrType<Uint8Array, Buffer, List<double>> { (value >> 16) & 15, (value >> 8) & 255, value & 255 });
        }

        public void pushBytes(OrType<Uint8Array, Buffer, List<double>> bytes)
        {
            bytes.forEach((b) => this.data.push(b));
        }

        public void writeByteLength(double length)
        {
            if (length >= 4294967296)
                throw new Error("string too large to encode: " + length);
            if (length >= 1 << 20)
            {
                this.pushByte(WA.Tags.BINARY_32);
                this.pushInt(length, 4);
            }
            else if (length >= 256)
            {
                this.pushByte(WA.Tags.BINARY_20);
                this.pushInt20(length);
            }
            else
            {
                this.pushByte(WA.Tags.BINARY_8);
                this.pushByte(length);
            }
        }

        public void writeStringRaw(string @string)
        {
            var bytes = Buffer.from(@string, "utf-8");
            this.writeByteLength(bytes.length);
            this.pushBytes(bytes);
        }

        public void writeJid(string left, string right)
        {
            this.pushByte(WA.Tags.JID_PAIR);
            left && left.Length > 0 ? this.writeString(left) : this.writeToken(WA.Tags.LIST_EMPTY);
            this.writeString(right);
        }

        public void writeToken(double token)
        {
            if (token < 245)
            {
                this.pushByte(token);
            }
            else if (token <= 500)
            {
                throw new Error("invalid token");
            }
        }

        public void writeString(string token, bool i = null)
        {
            if (token == "c.us")
                token = "s.whatsapp.net";
            var tokenIndex = WA.SingleByteTokens.indexOf(token);
            if (!i && token == "s.whatsapp.net")
            {
                this.writeToken(tokenIndex);
            }
            else if (tokenIndex >= 0)
            {
                if (tokenIndex < WA.Tags.SINGLE_BYTE_MAX)
                {
                    this.writeToken(tokenIndex);
                }
                else
                {
                    var overflow = tokenIndex - WA.Tags.SINGLE_BYTE_MAX;
                    var dictionaryIndex = overflow >> 8;
                    if (dictionaryIndex < 0 || dictionaryIndex > 3)
                    {
                        throw new Error("double byte dict token out of range: " + token + ", " + tokenIndex);
                    }

                    this.writeToken(WA.Tags.DICTIONARY_0 + dictionaryIndex);
                    this.writeToken(overflow % 256);
                }
            }
            else if (token)
            {
                var jidSepIndex = token.indexOf("@");
                if (jidSepIndex <= 0)
                {
                    this.writeStringRaw(token);
                }
                else
                {
                    this.writeJid(token.slice(0, jidSepIndex), token.slice(jidSepIndex + 1, token.Length));
                }
            }
        }

        public void writeAttributes(OrType<Record<string, string>, string> attrs, List<string> keys)
        {
            if (!attrs)
            {
                return;
            }

            keys.forEach((key) =>
            {
                this.writeString(key);
                this.writeString(attrs[key]);
            });
        }

        public void writeListStart(double listSize)
        {
            if (listSize == 0)
            {
                this.pushByte(WA.Tags.LIST_EMPTY);
            }
            else if (listSize < 256)
            {
                this.pushBytes(new OrType<Uint8Array, Buffer, List<double>> { WA.Tags.LIST_8, listSize });
            }
            else
            {
                this.pushBytes(new OrType<Uint8Array, Buffer, List<double>> { WA.Tags.LIST_16, listSize });
            }
        }

        public void writeChildren(OrType<string, List<WA.Node>, Buffer, Object> children)
        {
            if (!children)
                return;
            if (TypeOf(children) == "string")
            {
                this.writeString(children, true);
            }
            else if (Buffer.isBuffer(children))
            {
                this.writeByteLength(children.length);
                this.pushBytes(children);
            }
            else if (Array.isArray(children))
            {
                this.writeListStart(children.length);
                children.forEach((c) => c && this.writeNode(c));
            }
            else if (TypeOf(children) == "object")
            {
                var buffer = WA.Message.encode(children as dynamic).finish();
                this.writeByteLength(buffer.length);
                this.pushBytes(buffer);
            }
            else
            {
                throw new Error("invalid children: " + children + " (" + TypeOf(children) + ")");
            }
        }

        public void getValidKeys(Object obj)
        {
            return obj ? Object.keys(obj).filter((key) => obj[key] != null && obj[key] != undefined) : new List<>();
        }

        public void writeNode(WA.Node node)
        {
            if (!node)
            {
                return;
            }
            else if (node.length != 3)
            {
                throw new Error("invalid node given: " + node);
            }

            var validAttributes = this.getValidKeys(node[1]);
            this.writeListStart(2 * validAttributes.length + 1 + (node[2] ? 1 : 0));
            this.writeString(node[0]);
            this.writeAttributes(node[1], validAttributes);
            this.writeChildren(node[2]);
        }

        public void write(dynamic data)
        {
            this.data = new List<double>();
            this.writeNode(data);
            return Buffer.from(this.data);
        }
    }
}