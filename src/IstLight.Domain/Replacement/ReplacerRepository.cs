using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using IstLight.Domain.Extensions;

namespace IstLight.Domain.Replacement
{
    public static class ReplacerRepository
    {
        public static IReadOnlyCollection<ITickerReplacer> GetAll()
        {
            return
                Assembly.GetAssembly(typeof(ITickerReplacer)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ITickerReplacer)))
                .Select(t => Activator.CreateInstance(t) as ITickerReplacer)
                .AsReadOnlyCollection();
        }
    }
}
