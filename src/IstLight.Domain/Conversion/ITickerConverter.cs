using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Conversion
{
    public interface ITickerConverter
    {
        string Format { get; }

        ConvertResult<byte[]> ToBytes(Ticker ticker);
        ConvertResult<Ticker> ToTicker(string name, byte[] data);
    }
}
