using System;

namespace IstLight.Services
{
    public interface INamedItem : IDisposable
    {
        string Name { get; }
    }
}
