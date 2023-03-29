using Anni.Modules.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Commands
{
    public class periodMessage : Command
    {
        public periodMessage()
        {
            _commandName = "periodmessage";
            OnMessage = (bot, msg) =>
            {
                if (msg.ChatMessage.Message.Split(" ").Length <= 2)
                {
                    bot.SendWhisper(msg.ChatMessage.Username, "Invalid Args");
                    return;
                }
                string[] args = msg.ChatMessage.Message.Split(" ").Skip(1).ToArray();
                int duration = -1;
                int.TryParse(args[0], out duration);
                string text = string.Join(" ", args.Skip(1));
                if (duration <= 0)
                {
                    bot.SendWhisper(msg.ChatMessage.Username, "Invalid Duration");
                    return;
                }
                ChannelEntry entry = new ChannelEntry()
                {
                    ChannelId = bot._channel,
                    message = new PeriodicMessage()
                    {
                        text = text,
                        duration = duration
                    }
                };
                _ = Task.Run(async () =>
                {
                    if (await entry.exists)
                        await entry.UpdateOneAsync();
                    else
                        await entry.UploadOne();
                    bot.ManageChannel(entry);
                });
                bot.SendMessage($"A message will now be sent every {duration} seconds with the text: {text}");
            };
        }
    }
}
