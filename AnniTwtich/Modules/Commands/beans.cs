using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class beans : Command
    {
        public beans()
        {
            _commandName = "beans";
            OnMessage = (bot, msg) =>
            {
                bot.SendMessage($"You have {new Random().Next(100,9999)} beans. You hoarder.");
            };
        }
    }
}
