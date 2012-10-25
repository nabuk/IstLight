using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain
{
    public delegate T ProxiedIndexer<out T>(int index);
}
