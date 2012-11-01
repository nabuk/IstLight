using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Services
{
    public interface ITickerProviderRepository : IBaseRepository<ITickerProvider> { }
    public interface ITickerConverterRepository : IBaseRepository<ITickerConverter> { }
    public interface ITickerTransformerRepository : IBaseRepository<ITickerTransformer> { }
    public interface IResultAnalyzerRepository : IBaseRepository<IResultAnalyzer> { }
    public interface ITickerService
    {
        IReadOnlyList<IAsyncResult<Ticker>> Load();
        IAsyncResult<bool> Save(Ticker ticker);
    }
}
