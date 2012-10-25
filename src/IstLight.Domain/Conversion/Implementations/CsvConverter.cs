using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Conversion.Implementations
{
    public class CsvConverter : ITickerConverter
    {
        public string Format
        {
            get { return "csv"; }
        }

        public ConvertResult<byte[]> ToBytes(Ticker ticker)
        {
            throw new NotImplementedException();
        }

        public ConvertResult<Ticker> ToTicker(string name, byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
