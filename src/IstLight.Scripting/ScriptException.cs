﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Scripting
{
    public class ScriptException : Exception
    {
        public ScriptException(string name, string extension, string message)
            : base(string.Format("{0}.{1}: {2}", name, extension, message)) { }

        public ScriptException(Script script, string message) : this(script.Name,script.Extension, message) { }
    }
}
