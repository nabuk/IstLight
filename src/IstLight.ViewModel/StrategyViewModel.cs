
namespace IstLight
{
    public class StrategyViewModel
    {
        private string content;

        public StrategyViewModel()
        {
            Caption = "Strategy";
        }

        public string Caption { get; private set; }

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
    }
}
