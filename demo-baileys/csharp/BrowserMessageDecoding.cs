/// This file was generated by C# converter tool
/// Any changes made to this file manually will be lost next time the file is regenerated.

using System.Linq;
using TypeScript.CSharp;

namespace Bailey
{
    interface BrowserMessagesInfo
    {
        (string encKey, string macKey) bundle { get; set; }

        string harFilePath { get; set; }
    }

    interface WSMessage
    {
        string /*send*/ type { get; set; }

        string data { get; set; }
    }
}