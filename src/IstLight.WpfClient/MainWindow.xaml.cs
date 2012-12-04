using System.Windows;

namespace IstLight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = string.Format("{0} v{1}.{2}",
                ExecutingAssembly.Name,
                ExecutingAssembly.Version.Major,
                ExecutingAssembly.Version.Minor);
        }
    }
}
