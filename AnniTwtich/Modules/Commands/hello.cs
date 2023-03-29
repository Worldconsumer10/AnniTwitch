using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class Hello : Command
    {
        public Hello()
        {
            _commandName = "hello";
            OnMessage = (bot,msg) =>
            {
                bot.SendMessage("No");
            };
        }
    }
}
