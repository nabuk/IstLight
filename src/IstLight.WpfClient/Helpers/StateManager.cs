﻿// Copyright 2012 Jakub Niemyjski
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
