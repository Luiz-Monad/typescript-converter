/// This file was generated by Java converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

package demo;

public class Encoder {
    
    public /*missing*/ Encoder() {
        this.setData(new ArrayList<Double>());
    }
    private ArrayList<Double> data;
    
    public ArrayList<Double> getData() {
        return data;
    }
    
    public void setData(ArrayList<Double> value) {
        this.data = value;
    }
    
    public void pushByte(double value) {
        ArrayExtension.push(this.getData(), value & 255);
    }
    
    public void pushInt(double value, double n) {
        pushInt(value, n, false);
    }
    
    public void pushInt(double value, double n, boolean littleEndian) {
        for (int i = 0; i < n; i++) {
            final double curShift = littleEndian ? i : n - 1 - i;
            ArrayExtension.push(this.getData(), (value >> (curShift * 8)) & 255);
        }
    }
    
    public void pushInt20(double value) {
        this.pushBytes(new /*missing*/(Array.asList(new AnyXXXXXX[]{(value >> 16) & 15, (value >> 8) & 255, value & 255})));
    }
    
    public void pushBytes(/*missing*/ bytes) {
        bytes.forEach(new IForEachCallback<AnyXXXXXX>(){
            
            public AnyXXXXXX invoke(AnyXXXXXX b, int index) {
                return ArrayExtension.push(getData(), b);
            }
        });
    }
    
    public void writeByteLength(double length) {
        if (length >= 4294967296) throw new Error("string too large to encode: " + length);
        if (length >= 1 << 20) {
            this.pushByte(WA.getTags().getBINARY_32());
            this.pushInt(length, 4.0);
        } else if (length >= 256) {
            this.pushByte(WA.getTags().getBINARY_20());
            this.pushInt20(length);
        } else {
            this.pushByte(WA.getTags().getBINARY_8());
            this.pushByte(length);
        }
    }
    
    public void writeStringRaw(String string) {
        final AnyXXXXXX bytes = Buffer.from(string, "utf-8");
        this.writeByteLength(bytes.length());
        this.pushBytes(bytes);
    }
    
    public void writeJid(String left, String right) {
        this.pushByte(WA.getTags().getJID_PAIR());
        left && left.length() > 0 ? this.writeString(left) : this.writeToken(WA.getTags().getLIST_EMPTY());
        this.writeString(right);
    }
    
    public void writeToken(double token) {
        if (token < 245) {
            this.pushByte(token);
        } else if (token <= 500) {
            throw new Error("invalid token");
        }
    }
    
    public void writeString(String token) {
        writeString(token, null);
    }
    
    public void writeString(String token, boolean i) {
        if (StringOperator.logicalCompare(token, "===", "c.us")) token = "s.whatsapp.net";
        final AnyXXXXXX tokenIndex = WA.getSingleByteTokens().indexOf(token);
        if (!i && StringOperator.logicalCompare(token, "===", "s.whatsapp.net")) {
            this.writeToken(tokenIndex);
        } else if (tokenIndex >= 0) {
            if (tokenIndex < WA.getTags().getSINGLE_BYTE_MAX()) {
                this.writeToken(tokenIndex);
            } else {
                final AnyXXXXXX overflow = tokenIndex - WA.getTags().getSINGLE_BYTE_MAX();
                final AnyXXXXXX dictionaryIndex = overflow >> 8;
                if (dictionaryIndex < 0 || dictionaryIndex > 3) {
                    throw new Error("double byte dict token out of range: " + token + ", " + tokenIndex);
                }
                this.writeToken(WA.getTags().getDICTIONARY_0() + dictionaryIndex);
                this.writeToken(overflow % 256);
            }
        } else if (token) {
            final AnyXXXXXX jidSepIndex = token.indexOf("@");
            if (jidSepIndex <= 0) {
                this.writeStringRaw(token);
            } else {
                this.writeJid(StringExtension.slice(token, 0.0, jidSepIndex), StringExtension.slice(token, jidSepIndex + 1, token.length()));
            }
        }
    }
    
    public void writeAttributes(final /*missing*/ attrs, ArrayList<String> keys) {
        if (!attrs) {
            return;
        }
        ArrayExtension.forEach(keys, new IForEachCallback<AnyXXXXXX>(){
            
            public AnyXXXXXX invoke(AnyXXXXXX key, int index) {
                writeString(key);
                writeString(attrs.get(key));
            }
        });
    }
    
    public void writeListStart(double listSize) {
        if (listSize == 0) {
            this.pushByte(WA.getTags().getLIST_EMPTY());
        } else if (listSize < 256) {
            this.pushBytes(new /*missing*/(Array.asList(new AnyXXXXXX[]{WA.getTags().getLIST_8(), listSize})));
        } else {
            this.pushBytes(new /*missing*/(Array.asList(new AnyXXXXXX[]{WA.getTags().getLIST_16(), listSize})));
        }
    }
    
    public void writeChildren(/*missing*/ children) {
        if (!children) return;
        if (StringOperator.logicalCompare(typeOf(children), "===", "string")) {
            this.writeString(children, true);
        } else if (Buffer.isBuffer(children)) {
            this.writeByteLength(children.length());
            this.pushBytes(children);
        } else if (Array.isArray(children)) {
            this.writeListStart(children.length());
            children.forEach(new IForEachCallback<AnyXXXXXX>(){
                
                public AnyXXXXXX invoke(AnyXXXXXX c, int index) {
                    return c && writeNode(c);
                }
            });
        } else if (StringOperator.logicalCompare(typeOf(children), "===", "object")) {
            final AnyXXXXXX buffer = WA.getMessage().encode(cast(children, AnyXXXXXX.class)).finish();
            this.writeByteLength(buffer.length());
            this.pushBytes(buffer);
        } else {
            throw new Error("invalid children: " + children + " (" + typeOf(children) + ")");
        }
    }
    
    public void getValidKeys(final Object obj) {
        return obj ? Object.keys(obj).filter(new IFilterCallback<AnyXXXXXX>(){
            
            public AnyXXXXXX invoke(AnyXXXXXX key, int index) {
                return obj.get(key) != null && obj.get(key) != undefined;
            }
        }) : new void();
    }
    
    public void writeNode(WA.Node node) {
        if (!node) {
            return;
        } else if (node.length() != 3) {
            throw new Error("invalid node given: " + node);
        }
        final void validAttributes = this.getValidKeys(node.get(1));
        this.writeListStart(2 * validAttributes.length() + 1 + (node.get(2) ? 1 : 0));
        this.writeString(node.get(0));
        this.writeAttributes(node.get(1), validAttributes);
        this.writeChildren(node.get(2));
    }
    
    public void write(AnyXXXXXX data) {
        this.setData(new ArrayList<Double>());
        this.writeNode(data);
        return Buffer.from(this.getData());
    }
}