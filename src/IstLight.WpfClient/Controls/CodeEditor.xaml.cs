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
    public partial class CodeEditor
    {
        PropertyChangeNotifier textPropertyNotifier;
        PropertyChangeNotifier syntaxHighlightingPropertyNotifier;
        public CodeEditor()
        {
            InitializeComponent();
            
            avalonTextEditor.TextChanged += delegate
            {
                if(Text != avalonTextEditor.Text)
                    Text = avalonTextEditor.Text;
            };

            textPropertyNotifier = new PropertyChangeNotifier(this, TextProperty);
            textPropertyNotifier.ValueChanged += delegate
            {
                if (Text != avalonTextEditor.Text)
                    avalonTextEditor.Text = Text;
            };

            syntaxHighlightingPropertyNotifier = new PropertyChangeNotifier(this, SyntaxHighlightingProperty);
            syntaxHighlightingPropertyNotifier.ValueChanged += delegate
            {
                if (string.IsNullOrWhiteSpace(SyntaxHighlighting))
                    return;
                using (Stream s = SyntaxHighlighting.ToStream())
                using (var xmlReader = new XmlTextReader(s))
                    avalonTextEditor.SyntaxHighlighting = HighlightingLoader.Load(xmlReader, HighlightingManager.Instance);
            };
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CodeEditor), new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty SyntaxHighlightingProperty =
            DependencyProperty.Register("SyntaxHighlighting", typeof(string), typeof(CodeEditor), new PropertyMetadata(""));

        public string SyntaxHighlighting
        {
            get { return (string)GetValue(SyntaxHighlightingProperty); }
            set { SetValue(SyntaxHighlightingProperty, value); }
        }
    }
}
