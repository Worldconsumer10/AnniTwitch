using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;

namespace Anni.Modules
{
    public class Command
    {
        public string _commandName { get; set; }
        public Action<ChannelBot,OnMessageReceivedArgs> OnMessage { get; set; }
        public static void FindAndExecute(ChannelBot client, OnMessageReceivedArgs receivedArgs)
        {
            _ = Task.Run(async () =>
            {
                string? firstWord = receivedArgs.ChatMessage.Message.Split(" ").FirstOrDefault();
                if (firstWord == null || !firstWord.StartsWith("a!") || firstWord.Split("a!").Length <= 0) return;
                string? command = firstWord.Split("a!")[1];
                if (command == null) return;
                var cmds = GetAllItems();
                Command? cmd = cmds.FirstOrDefault(r => r != null && command.ToLower() == r._commandName.ToLower());
                if (cmd != null)
                {
                    cmd.OnMessage(client,receivedArgs);
                }
            });
        }
        public static List<string> GetAllCommandNames()
        {
            var cmds = GetAllItems();
            List<string> val = new List<string>();
            foreach (var cmd in cmds)
            {
                if (cmd == null) continue;
                val.Add(cmd._commandName);
            }
            return val;
        }
        static IEnumerable<Command?> GetAllItems()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Command)))
                .Select(type => Activator.CreateInstance(type) as Command);
        }
    }
}
