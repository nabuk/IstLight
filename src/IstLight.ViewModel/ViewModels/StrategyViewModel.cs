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
