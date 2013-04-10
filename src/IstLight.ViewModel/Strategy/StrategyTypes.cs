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
using System.Linq;
using System.Text;

namespace IstLight.Strategy
{
    public class StrategyTypes
    {
        private readonly Dictionary<string,string> extensionWithName;
        private readonly Dictionary<string, string> extensionWithSyntaxHighlighting;
        private readonly Dictionary<string, string> extensionWithExampleScript;

        public StrategyTypes(
            IEnumerable<KeyValuePair<string,string>> extensionWithName,
            IEnumerable<KeyValuePair<string,string>> extensionWithSyntaxHighlighting,
            IEnumerable<KeyValuePair<string, string>> extensionWithExampleScript)
        {
            this.extensionWithName = extensionWithName.ToDictionary(x => x.Key, x => x.Value);
            this.extensionWithSyntaxHighlighting = extensionWithSyntaxHighlighting.ToDictionary(x => x.Key, x => x.Value);
            this.extensionWithExampleScript = extensionWithExampleScript.ToDictionary(x => x.Key, x => x.Value);
        }

        public IEnumerable<KeyValuePair<string, string>> ExtensionWithName
        {
            get
            {
                return extensionWithName;
            }
        }

        public string GetSyntaxHighlighting(string strategyExtension)
        {
            return extensionWithSyntaxHighlighting[strategyExtension];
        }

        public string GetExampleScript(string strategyExtension)
        {
            return extensionWithExampleScript[strategyExtension];
        }
    }
}
