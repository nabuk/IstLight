using System;
using System.Windows;
using IstLight.Dictionaries;

namespace IstLight
{
    public class WindowAdapter : IWindow
    {
        protected readonly Window wpfWindow;

        public WindowAdapter(Window wpfWindow)
        {
            if (wpfWindow == null)
            {
                throw new ArgumentNullException("window");
            }

            this.wpfWindow = wpfWindow;
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
