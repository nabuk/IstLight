using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight
{
    public class ResultOrError<T>
    {
        public ResultOrError() { }
        public ResultOrError(Func<T> unsafeDelegate)
        {
            try { Result = unsafeDelegate(); }
            catch (Exception ex) { Error = ex; }
        }
        public T Result { get; set; }
        public Exception Error { get; set; }
    }
}
