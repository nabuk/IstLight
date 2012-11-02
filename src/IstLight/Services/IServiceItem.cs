using System;

namespace IstLight.Services
{
    public interface IServiceItem : IDisposable
    {
        string Name { get; }
    }
}
