using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class Simp : Command
    {
        public Simp()
        {
            _commandName = "simp";
            OnMessage = (bot, msg) =>
            {
                if (msg.ChatMessage.Message.Split(" ").Length <= 1) return;
                var user = msg.ChatMessage.Message.Split(" ")[1];
                var atuser = bot.users.FirstOrDefault(u => u.ToLower() == user.ToLower());
                bot.SendMessage($"{msg.ChatMessage.DisplayName} is simping for {(atuser != null ? atuser : user)}");
            };
        }
    }
}
