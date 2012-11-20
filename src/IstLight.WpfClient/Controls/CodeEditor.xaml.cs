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
