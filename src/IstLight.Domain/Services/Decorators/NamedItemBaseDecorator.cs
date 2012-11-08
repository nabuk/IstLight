using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services.Decorators
{
    public abstract class NamedItemBaseDecorator<T> : INamedItem
        where T : INamedItem
    {
        protected readonly T itemToDecorate;

        public NamedItemBaseDecorator(T itemToDecorate)
        {
            this.itemToDecorate = itemToDecorate;
        }

        public virtual string Name
        {
            get { return this.itemToDecorate.Name; }
        }
    }
}
