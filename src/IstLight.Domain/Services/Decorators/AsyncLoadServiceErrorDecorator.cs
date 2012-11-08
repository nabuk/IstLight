using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IstLight;

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

        private static Func<T, T> ErrorDecoratorDelegate<T>(IErrorReporter errorReporter)
            where T : INamedItem
        {
            Type decoratorType =
                typeof(T).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(NamedItemBaseErrorDecorator<T>)) && !t.IsAbstract)
                    .FirstOrDefault();
            if (decoratorType == null)
                return null;

            return x => (T)Activator.CreateInstance(decoratorType, x, errorReporter);
        }
    }
}
