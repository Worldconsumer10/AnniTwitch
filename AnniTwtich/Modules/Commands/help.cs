using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class Help : Command
    {
        public Help()
        {
            _commandName = "help";
            OnMessage = (bot, msg) =>
            {
                bot.SendMessage($"Command List: a!{string.Join(", a!",GetAllCommandNames())}");
            };
        }
    }
}
