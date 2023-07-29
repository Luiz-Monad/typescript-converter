using System;
using System.Collections.Generic;
using System.Text;

namespace TypeScript.Syntax.Converter
{
    /// <summary>
    /// Defines convert hooks interface.
    /// </summary>
    public interface IConverterHook
    {
        void Convert(Node source, object target);
    }
}
