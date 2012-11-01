using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Synchronization
{
    public interface ISimpleTickerQuote : IDate
    {
        double Value { get; }
        double? Volume { get; }
    }
}
