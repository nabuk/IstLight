using System;
using System.Windows;
using System.Windows.Controls;

namespace IstLight.Helpers
{
    public class StateManager : DependencyObject
    {
        public static string GetVisualState(DependencyObject obj)
        {
            return (string)obj.GetValue(VisualStateProperty);
        }

        public static void SetVisualState(DependencyObject obj, string value)
        {
            obj.SetValue(VisualStateProperty, value);
        }

        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.RegisterAttached(
            "VisualState",
            typeof(string),
            typeof(StateManager),
            new PropertyMetadata((s, e) =>
            {
                var newState = (string)e.NewValue;
                var ctrl = s as Control;
                if(ctrl == null)
                    throw new InvalidOperationException("This attached property only supports types derived from Control.");
                System.Windows.VisualStateManager.GoToState(ctrl, newState, true);
            }));
    }
}
