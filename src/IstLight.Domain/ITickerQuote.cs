using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain
{
    public interface ITickerQuote : IDate
    {
        double Open { get; }
        double Close { get; }
        double High { get; }
        double Low { get; }

        double? Volume { get; }
    }
}
