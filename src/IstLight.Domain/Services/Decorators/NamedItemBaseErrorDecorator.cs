using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Services.Decorators
{
    public abstract class NamedItemBaseErrorDecorator<T> : NamedItemBaseDecorator<T>
        where T : INamedItem
    {
        protected readonly IErrorReporter errorReporter;

        public NamedItemBaseErrorDecorator(T itemToDecorate, IErrorReporter errorReporter) : base(itemToDecorate)
        {
            this.errorReporter = errorReporter;
        }

        protected IAsyncResult<V> Decorate<V>(IAsyncResult<V> arItem)
        {
            arItem.AddCallback(x => errorReporter.AddIfNotNull(x.Error));
            return arItem;
        }
        protected ValueOrError<V> Decorate<V>(ValueOrError<V> item)
        {
            errorReporter.AddIfNotNull(item.Error);
            return item;
        }
    }
}
