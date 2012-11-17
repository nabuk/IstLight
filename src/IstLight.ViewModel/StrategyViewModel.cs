using System;
using IstLight.Strategy;

namespace IstLight
{
    public class StrategyViewModel
    {
        private string content;

        internal bool CompileStrategy(out StrategyBase strategy)
        {
            throw new NotImplementedException();
        }

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
