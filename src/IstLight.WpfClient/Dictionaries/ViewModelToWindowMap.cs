using System;
using System.Collections.Generic;
using System.Windows;
using IstLight.ViewModels;
using IstLight.Views;

namespace IstLight.Dictionaries
{
    public static class ViewModelToWindowMap
    {
        private static readonly IDictionary<Type, Type> map = new Dictionary<Type, Type>
        {
            { typeof(SimulationProgressViewModel), typeof(SimulationProgressView) },
            { typeof(SimulationResultViewModel), typeof(SimulationResultView) },
            { typeof(AboutViewModel), typeof(AboutView) }
        };

        public static Window Create(object viewModel)
        {
            return (Window)Activator.CreateInstance(map[viewModel.GetType()]);
        }
    }
}
