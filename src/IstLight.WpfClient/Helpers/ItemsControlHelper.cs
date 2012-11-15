﻿using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace IstLight.Helpers
{
    public class ItemsControlHelper : DependencyObject
    {
        public static readonly DependencyProperty AutoScrollToEndProperty
            = DependencyProperty.RegisterAttached(
                "AutoScrollToEnd",
                typeof(bool),
                typeof(ItemsControlHelper),
                new UIPropertyMetadata(default(bool), OnAutoScrollToEndChanged));

        public static bool GetAutoScrollToEnd(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToEndProperty);
        }

        public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToEndProperty, value);
        }

        public static void OnAutoScrollToEndChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = s as ItemsControl;
            if (ctrl == null)
                throw new InvalidOperationException("This attached property only supports ItemsControl or derived types.");

            var data = ctrl.Items as INotifyCollectionChanged;
            if (data == null)
                throw new InvalidOperationException("Collection does not support change notifications.");

            Control parentCtrl = ctrl;
            while (parentCtrl != null && parentCtrl.GetType() != typeof(ScrollViewer))
                parentCtrl = parentCtrl.Parent as Control;
            ScrollViewer sv = parentCtrl as ScrollViewer;

            var scrollToEndHandler = new NotifyCollectionChangedEventHandler(
                (s1, e1) =>
                {
                    if (e1.Action == NotifyCollectionChangedAction.Add && sv != null)
                        sv.ScrollToBottom();
                });

            if ((bool)e.NewValue)
                data.CollectionChanged += scrollToEndHandler;
            else
                data.CollectionChanged -= scrollToEndHandler;
        }
    }
}
