using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace IstLight.Dictionaries
{
    public static class ViewModelToWindowMap
    {
        private static IDictionary<Type, Type> map = new Dictionary<Type, Type>
        {
            { typeof(SimulationProgressViewModel), typeof(SimulationProgressView) }
        };

        public static Window Create(object viewModel)
        {
            return (Window)Activator.CreateInstance(map[viewModel.GetType()]);
        }
    }
}
