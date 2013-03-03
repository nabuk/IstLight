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
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using IstLight.Dictionaries;

namespace IstLight
{
    public class WindowAdapter : IWindow
    {
        protected readonly Window wpfWindow;
        protected readonly BlurEffect blurEffect;

        public WindowAdapter(Window wpfWindow)
        {
            if (wpfWindow == null)
                throw new ArgumentNullException("wpfWindow");

            this.wpfWindow = wpfWindow;


            blurEffect = new BlurEffect { Radius = 0 };
            this.WpfWindow.Effect = blurEffect;
        }

        #region IWindow Members

        public virtual void Close()
        {
            this.wpfWindow.Close();
        }

        public virtual IWindow CreateChild(object viewModel)
        {
            var child = ViewModelToWindowMap.Create(viewModel);
            child.Owner = this.wpfWindow;
            child.Closing += (s, e) =>
                {
                    if (!System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                        (s as Window).Owner = null;
                };
            child.DataContext = viewModel;
            WindowAdapter.ConfigureBehavior(child);
            return new WindowAdapter(child);
        }

        public virtual void Show()
        {
            this.wpfWindow.Show();
        }

        public virtual bool? ShowDialog()
        {
            return this.wpfWindow.ShowDialog();
        }

        private bool isBlurred;
        public bool IsBlurred
        {
            get
            {
                return this.isBlurred;
            }
            set
            {
                this.blurEffect.Radius = value ? 5 : 0;
                this.isBlurred = value;
            }
        }

        #endregion

        protected Window WpfWindow
        {
            get { return this.wpfWindow; }
        }

        private static void ConfigureBehavior(Window wnd)
        {
            wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //cw.CommandBindings.Add(new CommandBinding(new RoutedCommand("Accept", cw.GetType()), (sender, e) => cw.DialogResult = true));
        }
    }
}
