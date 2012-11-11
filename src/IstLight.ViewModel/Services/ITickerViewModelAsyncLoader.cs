﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface ITickerViewModelLoader
    {
        event Action<TickerViewModel> LoadingTicker;
    }
}