using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services
{
    public interface IErrorReporter
    {
        void Add(string error);
        void Add(Exception error);
    }
}
