using System.Collections.Generic;
using System.Linq;

namespace IstLight.Commands
{
    public class GlobalCommandContainer
    {
        private readonly Dictionary<string, IGlobalCommand> commandsDict;

        public GlobalCommandContainer(IEnumerable<IGlobalCommand> commands)
        {
            this.commandsDict = commands.ToDictionary(k => k.Key, v => v);
        }

        public IGlobalCommand this[string key]
        {
            get
            {
                return commandsDict[key];
            }
        }
    }
}
