using System.Windows.Controls;

namespace IstLight.Views
{
    /// <summary>
    /// Interaction logic for SectionHeaderView.xaml
    /// </summary>
    public partial class SectionHeaderView : UserControl
    {
        public SectionHeaderView()
        {
            InitializeComponent();
            
        }

        public string Title
        {
            get { return tbTitle.Text; }
            set { tbTitle.Text = value; }
        }
    }
}
