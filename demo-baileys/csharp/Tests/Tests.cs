/// This file was generated by C# converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

using System.Linq;
using TypeScript.CSharp;

namespace Bailey
{
    class MyClass : Object
    {
        public MyClass()
        {
            this.didDoWork = false;
            this.values = new Hashtable<String, double>();
            this.counter = 0;
        }

        public bool didDoWork
        {
            get;
            set;
        }

        public Hashtable<String, double> values
        {
            get;
            set;
        }

        public int counter
        {
            get;
            set;
        }

        async public void myFunction()
        {
            if (this.didDoWork)
                return;
            await new Promise((resolve) => setTimeout(resolve, DEFAULT_WAIT));
            if (this.didDoWork)
            {
                throw new Error("work already done");
            }

            this.didDoWork = true;
        }

        async public void myKeyedFunction(String key)
        {
            if (!this.values[key])
            {
                await new Promise((resolve) => setTimeout(resolve, DEFAULT_WAIT));
                if (this.values[key])
                    throw new Error("value already set for " + key);
                this.values[key] = Math.floor(Math.random() * 100);
            }

            return this.values[key];
        }

        async public void myQueingFunction(String key)
        {
            await new Promise((resolve) => setTimeout(resolve, DEFAULT_WAIT));
        }

        async public void myErrorFunction()
        {
            await new Promise((resolve) => setTimeout(resolve, 100));
            this.counter += 1;
            if (this.counter % 2 == 0)
            {
                throw new Error("failed");
            }
        }
    }
}