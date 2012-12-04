// Copyright 2012 Jakub Niemyjski
//
// This file is part of IstLight.
//
// IstLight is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// IstLight is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with IstLight.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;

namespace IstLight.Services.Decorators
{
    public class AsyncLoadServiceErrorDecorator<T> : AsyncLoadServiceDecorator<T>
        where T : INamedItem
    {
        public AsyncLoadServiceErrorDecorator(IAsyncLoadService<T> loadService, IErrorReporter errorReporter)
            : base(loadService, PrepareFactory(errorReporter)) { }

        private static DelegateAsyncResultDecoratorFactory<T> PrepareFactory(IErrorReporter errorReporter)
        {
            return new DelegateAsyncResultDecoratorFactory<T>(arToDecorate =>
                new AsyncResultErrorDecorator<T>(
                    arToDecorate,
                    errorReporter,
                    ErrorDecoratorDelegate<T>(errorReporter)));
        }

        private static Func<V, V> ErrorDecoratorDelegate<V>(IErrorReporter errorReporter)
            where V : INamedItem
        {
            Type decoratorType =
                typeof(V).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(NamedItemBaseErrorDecorator<V>)) && !t.IsAbstract)
                    .FirstOrDefault();
            if (decoratorType == null)
                return null;

            return x => (V)Activator.CreateInstance(decoratorType, x, errorReporter);
        }
    }
}
