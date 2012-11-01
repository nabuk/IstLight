using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Services
{
    public interface IRepositoryItem : IDisposable
    {
        string Name { get; }
    }
}
