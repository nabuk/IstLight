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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using IstLight.Services;
using IstLight.ViewModels;
using winForms = System.Windows.Forms;

namespace IstLight.Strategy
{
    public class StrategyFileCommands : ViewModelVisitor<StrategyExplorerViewModel>
    {
        private int lastIndex = 0;
        private StrategyViewModel previousStrategy;

        protected override void OnAfterAttach()
        {
            ViewModel.SelectedStrategyChanged += ViewModel_SelectedStrategyChanged;
            NewCommand.Execute(ViewModel.StrategyTypes.ExtensionWithName.First().Key);
        }

        void ViewModel_SelectedStrategyChanged(StrategyViewModel obj)
        {
            if (previousStrategy != null)
                previousStrategy.ChangedPropertyChanged -= Strategy_ChangedPropertyChanged;
            obj.ChangedPropertyChanged += Strategy_ChangedPropertyChanged;
            previousStrategy = obj;
            RaiseCanExecuteSaveCommandChanged();
        }

        void Strategy_ChangedPropertyChanged(bool obj)
        {
            RaiseCanExecuteSaveCommandChanged();
        }

        void RaiseCanExecuteSaveCommandChanged()
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void ShowError(string message, string caption = "Error")
        {
            winForms.MessageBox.Show(message, caption, winForms.MessageBoxButtons.OK, winForms.MessageBoxIcon.Error);
        }

        private bool SaveFile(string path, string content)
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    writer.Write(content);
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                return false;
            }
        }

        private bool SaveWithoutDialog(StrategyViewModel strategy)
        {
            if (SaveFile(strategy.Path, strategy.Content))
            {
                strategy.SetNotChanged();
                return true;
            }
            return false;
        }

        private bool SaveWithDialog(StrategyViewModel strategy)
        {
            var langName = ViewModel.StrategyTypes.ExtensionWithName.First(x => x.Key == strategy.Extension).Value;

            var dlg = new winForms.SaveFileDialog
            {
                FileName = strategy.Name + "." + strategy.Extension,
                Filter = string.Format("{0} files (*.{1})|*.{1}", langName, strategy.Extension),
                AddExtension = true,
                ValidateNames = true
            };
            if (dlg.ShowDialog() == winForms.DialogResult.OK)
            {
                if (SaveFile(dlg.FileName, strategy.Content))
                {
                    strategy.Path = dlg.FileName;
                    strategy.SetNotChanged();
                    return true;
                }
            }
            return false;
        }

        private bool ForcedSave(StrategyViewModel strategy)
        {
            return strategy.Path == null ? SaveWithDialog(strategy) : SaveWithoutDialog(strategy);
        }

        #region Command handlers
        private void NewCommandHandler(string extension)
        {
            ViewModel.AddAndSelect(
                new StrategyViewModel(
                    "New" + (++lastIndex),
                    extension,
                    ViewModel.StrategyTypes.GetSyntaxHighlighting(extension)));
        }

        private void OpenCommandHandler()
        {
            string fileExtensions =
                ViewModel.StrategyTypes.ExtensionWithName
                .Select(x => "*."+x.Key)
                .Aggregate((s1,s2) => s1+";"+s2);

            var dlg = new winForms.OpenFileDialog
            {
                Filter = string.Format("Supported files ({0})|{1}", fileExtensions.Replace(';', ','), fileExtensions),
                CheckFileExists = true,
                Multiselect = true
            };
            if (dlg.ShowDialog() == winForms.DialogResult.OK)
            {
                List<string> cannotLoad = new List<string>();
                List<StrategyViewModel> strategies = new List<StrategyViewModel>();
                foreach (var fileName in dlg.FileNames)
                {
                    try
                    {
                        string content;
                        using (var reader = new StreamReader(fileName))
                        {
                            content = reader.ReadToEnd();
                        }
                        string name = Path.GetFileNameWithoutExtension(fileName);
                        string extension = Path.GetExtension(fileName);
                        var strategy = new StrategyViewModel(name, extension, ViewModel.StrategyTypes.GetSyntaxHighlighting(extension))
                        {
                            Content = content
                        };
                        strategy.SetNotChanged();
                    }
                    catch (Exception)
                    {
                        cannotLoad.Add(Path.GetFileName(fileName));
                    }
                }

                foreach (var strategy in strategies)
                    ViewModel.AddAndSelect(strategy);

                if (cannotLoad.Count != 0)
                    ShowError(string.Format("Cannot load following files:{0}",
                        cannotLoad.Aggregate("", (s1, s2) => s1 + Environment.NewLine + s2)));
            }
        }

        private void SaveCommandHandler()
        {
            SaveWithoutDialog(ViewModel.SelectedStrategy);
        }

        private void SaveAsCommandHandler()
        {
            SaveWithDialog(ViewModel.SelectedStrategy);
        }

        private void CloseCommandHandler()
        {
            var strategy = ViewModel.SelectedStrategy;
            if (strategy.Changed)
            {
                string dialogMessage = string.Format("Save file \"{0}.{1}\" ?", strategy.Name, strategy.Extension);
                switch (winForms.MessageBox.Show(dialogMessage, "Save", winForms.MessageBoxButtons.YesNoCancel, winForms.MessageBoxIcon.Question))
                {
                    case winForms.DialogResult.Cancel: return;

                    case winForms.DialogResult.Yes:
                        if (ForcedSave(strategy))
                            break;
                        else
                            return;
                    case winForms.DialogResult.No: break;
                }
            }

            bool onlyOne = ViewModel.Strategies.Count == 1;
            if (onlyOne)
            {
                NewCommand.Execute(strategy.Extension);
                ViewModel.Remove(strategy);
            }
        }
        #endregion

        #region CommandCanExecute handlers

        private bool CanSaveCommandHandler()
        {
            return ViewModel.SelectedStrategy.Changed && ViewModel.SelectedStrategy.Path != null;
        }

        #endregion

        public StrategyFileCommands()
        {
            this.NewCommand = new DelegateCommand<string>(NewCommandHandler);
            this.OpenCommand = new DelegateCommand(OpenCommandHandler);
            this.SaveCommand = new DelegateCommand(SaveCommandHandler, CanSaveCommandHandler);
            this.SaveAsCommand = new DelegateCommand(SaveAsCommandHandler);
            this.CloseCommand = new DelegateCommand(CloseCommandHandler);
        }

        #region Commands
        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }
        #endregion
    }
}
