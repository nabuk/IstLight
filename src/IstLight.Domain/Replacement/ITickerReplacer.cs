using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Replacement
{
    public interface ITickerReplacer
    {
        string Name { get; }
	    
        bool CanReplace(Ticker ticker);
	    Ticker Replace(Ticker ticker);
    }
}
