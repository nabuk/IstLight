using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Xml;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace IstLight
{
    /// <summary>
    /// Interaction logic for StrategyView.xaml
    /// </summary>
    public partial class StrategyView : UserControl
    {
        public StrategyView()
        {
            InitializeComponent();

            StreamResourceInfo sri = Application.GetResourceStream(new Uri("Dictionaries/SyntaxHighlighting.Python.xshd", UriKind.Relative));
            if (sri != null)
                using (Stream s = sri.Stream)
                {
                    avalonTextEditor.SyntaxHighlighting = HighlightingLoader.Load(new XmlTextReader(s), HighlightingManager.Instance);
                }
        }
    }
}
