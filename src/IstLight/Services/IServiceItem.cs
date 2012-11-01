using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface IServiceItem : IDisposable
    {
        string Name { get; }
    }
}
