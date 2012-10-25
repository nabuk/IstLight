using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Strategy
{
    public class EmptyStrategy : StrategyBase
    {
        public override bool Run() { return true; }
    }
}
