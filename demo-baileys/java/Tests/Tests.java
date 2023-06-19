/// This file was generated by Java converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

package demo;

class MyClass {
    
    /*missing*/ MyClass() {
        this.setDidDoWork(false);
        this.setValues(new HashMap<String, Double>());
        this.setCounter(0);
    }
    private boolean didDoWork;
    
    public boolean getDidDoWork() {
        return didDoWork;
    }
    
    public void setDidDoWork(boolean value) {
        this.didDoWork = value;
    }
    private HashMap<String, Double> values;
    
    public HashMap<String, Double> getValues() {
        return values;
    }
    
    public void setValues(HashMap<String, Double> value) {
        this.values = value;
    }
    private int counter;
    
    public int getCounter() {
        return counter;
    }
    
    public void setCounter(int value) {
        this.counter = value;
    }
    
    public void myFunction() {
        if (this.getDidDoWork()) return;
        new Promise(new ArrowFnXXXXXX(){
            
            public AnyXXXXXX invoke(AnyXXXXXX resolve) {
                return setTimeout(resolve, DEFAULT_WAIT);
            }
        });
        if (this.getDidDoWork()) {
            throw new Error("work already done");
        }
        this.setDidDoWork(true);
    }
    
    public void myKeyedFunction(String key) {
        if (!this.getValues().get(key)) {
            new Promise(new ArrowFnXXXXXX(){
                
                public AnyXXXXXX invoke(AnyXXXXXX resolve) {
                    return setTimeout(resolve, DEFAULT_WAIT);
                }
            });
            if (this.getValues().get(key)) throw new Error("value already set for " + key);
            this.getValues().put(key, Math.floor(Math.random() * 100));
        }
        return this.getValues().get(key);
    }
    
    public void myQueingFunction(String key) {
        new Promise(new ArrowFnXXXXXX(){
            
            public AnyXXXXXX invoke(AnyXXXXXX resolve) {
                return setTimeout(resolve, DEFAULT_WAIT);
            }
        });
    }
    
    public void myErrorFunction() {
        new Promise(new ArrowFnXXXXXX(){
            
            public AnyXXXXXX invoke(AnyXXXXXX resolve) {
                return setTimeout(resolve, 100.0);
            }
        });
        this.setCounter(this.getCounter() + 1);
        if (this.getCounter() % 2 == 0) {
            throw new Error("failed");
        }
    }
}