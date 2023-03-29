using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class respect : Command
    {
        public respect()
        {
            _commandName = "respect";
            OnMessage = (bot, msg) =>
            {
                bot.SendMessage("As if id give you any respect.");
            };
        }
    }
}
