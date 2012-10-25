using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using IstLight.Domain.Extensions;

namespace IstLight.Domain.Conversion
{
    public static class ConverterRepository
    {
        public static IReadOnlyCollection<ITickerConverter> GetAll()
        {
            return
                Assembly.GetAssembly(typeof(ITickerConverter)).GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ITickerConverter)))
                .Select(t => Activator.CreateInstance(t) as ITickerConverter)
                .AsReadOnlyCollection();
        }
    }
}
