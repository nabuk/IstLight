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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using IstLight.Helpers;

namespace IstLight.Controls
{
    /// <summary>
    /// Interaction logic for StrategyEditor.xaml
    /// </summary>
    public partial class CodeEditor
    {
        PropertyChangeNotifier notifier;
        public CodeEditor()
        {
            InitializeComponent();
            
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("IstLight;component/dictionaries/SyntaxHighlighting.Python.xshd", UriKind.Relative));
            if (sri != null)
                using (Stream s = sri.Stream)
                {
                    avalonTextEditor.SyntaxHighlighting = HighlightingLoader.Load(new XmlTextReader(s), HighlightingManager.Instance);
                }

            avalonTextEditor.TextChanged += delegate
            {
                if(Text != avalonTextEditor.Text)
                    Text = avalonTextEditor.Text;
            };

            notifier = new PropertyChangeNotifier(this, TextProperty);
            notifier.ValueChanged += delegate
            {
                if (Text != avalonTextEditor.Text)
                    avalonTextEditor.Text = Text;
            };
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CodeEditor), new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
