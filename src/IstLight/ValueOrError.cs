using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight
{
    public class ValueOrError<T>
    {
        public ValueOrError() { }
        public ValueOrError(Func<T> unsafeDelegate)
        {
            try { Value = unsafeDelegate(); }
            catch (Exception ex) { Error = ex; }
        }
        public T Value { get; set; }
        public Exception Error { get; set; }

        public bool IsError { get { return Error != null; } }
    }
}
