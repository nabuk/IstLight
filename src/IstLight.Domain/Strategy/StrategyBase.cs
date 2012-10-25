using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight.Domain.Synchronization;

namespace IstLight.Domain.Strategy
{
    public abstract class StrategyBase : IDisposable
    {
        protected internal IQuoteContext QuoteContext { get; internal set; }
        protected internal IWalletContext WalletContext { get; internal set; }

        #region To override
        public virtual bool Initialize() { return true; }

        public abstract bool Run();

        public virtual string LastError { get { return null; } }

        public virtual void Dispose() { }
        #endregion
    }
}
