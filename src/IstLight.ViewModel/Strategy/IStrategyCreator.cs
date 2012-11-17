using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Strategy
{
    public interface IStrategyCreator
    {
        StrategyBase CreateStrategy();
    }
}
