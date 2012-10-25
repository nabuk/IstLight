using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Conversion
{
    public struct ConvertResult<T>
    {
        public string Error { get; set; }
        public T Value { get; set; }
        public bool Succeeded { get { return string.IsNullOrWhiteSpace(Error); } }
    }
}
