/// This file was generated by Java converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

package demo;

import demo.Decoder;

interface BrowserMessagesInfo {
    
    Pair<String, String> getBundle();
    
    void setBundle(Pair<String, String> value);
    
    String getHarFilePath();
    
    void setHarFilePath(String value);
}
interface WSMessage {
    
    /*missing*/ getType();
    
    void setType(/*missing*/ value);
    
    String getData();
    
    void setData(String value);
}