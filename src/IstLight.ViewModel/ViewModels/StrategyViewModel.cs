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
using GalaSoft.MvvmLight;

namespace IstLight.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        private string name = "Default";
        private string orgContent = string.Empty;
        private string content = string.Empty;
        

        private void StrategyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name": RaisePropertyChanged<string>(() => Caption); break;
                case "Changed": RaisePropertyChanged<string>(() => Caption); ChangedPropertyChanged(Changed); break;
            }
        }

        public StrategyViewModel(string name, string extension, string syntaxHighlighting)
        {
            this.name = name;
            this.Extension = extension;
            this.SyntaxHighlighting = syntaxHighlighting;

            base.PropertyChanged += StrategyViewModel_PropertyChanged;
        }

        public string Name
        {
            get { return name; }
            internal set
            {
                if (name == value)
                    return;
                name = value;
                RaisePropertyChanged<string>(() => Name);
            }
        }
        public string Extension { get; private set; }
        public string Content
        {
            get { return content ?? ""; }
            set
            {
                if (value == content)
                    return;

                content = value;
                RaisePropertyChanged<bool>(() => Changed);
            }
        }

        public bool Changed
        {
            get { return orgContent != content; }
        }
        public string Caption
        {
            get
            {
                return (Changed ? "* ":"")
                    + name
                    + (string.IsNullOrWhiteSpace(Extension) ? "" : ".") + Extension;
            }
        }

        public string SyntaxHighlighting { get; private set; }
        public event Action<bool> ChangedPropertyChanged = delegate { };

        internal string Path { get; set; }
        internal void SetNotChanged()
        {
            orgContent = content;
            RaisePropertyChanged<bool>(() => Changed);
        }
        internal Script ToScript()
        {
            return new Script(name, Extension, content);
        }
    }
}
