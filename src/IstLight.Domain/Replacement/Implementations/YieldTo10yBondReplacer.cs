using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Replacement
{
    public class YieldTo10yBondReplacer : ITickerReplacer
    {
        public string Name
        {
            get { return "Yield to 10 year bond"; }
        }

        public bool CanReplace(Ticker ticker)
        {
            return true;
        }

        public Ticker Replace(Ticker ticker)
        {
            throw new NotImplementedException();
        }
    }
}
