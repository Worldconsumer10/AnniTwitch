using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class Dice : Command
    {
        public Dice()
        {
            _commandName = "dice";
            OnMessage = (bot, msg) =>
            {
                bot.SendMessage($"You rolled a {new Random().Next(1, 6)}");
            };
        }
    }
}
