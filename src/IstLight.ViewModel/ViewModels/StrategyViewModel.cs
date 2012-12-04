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

using GalaSoft.MvvmLight;

namespace IstLight.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        private string name;
        private string content;
        private string extension;

        public StrategyViewModel()
        {
            name = "Strategy";
            extension = "py";
        }

        public string Caption
        {
            get
            {
                return name + (string.IsNullOrWhiteSpace(extension) ? "" : ".") + extension;
            }
        }

        public string Content
        {
            get { return content ?? ""; }
            set
            {
                if (value == content)
                    return;

                content = value;
            }
        }

        internal Script ToScript()
        {
            return new Script(name, extension, content);
        }
    }
}
