using TwitchLib.Client.Models;
using TwitchLib.Client;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;
using TwitchLib.Communication.Clients;
using System.Text.Json;
using TwitchLib.Client.Events;
using TwitchLib.Client.Interfaces;
using Anni.Modules;

namespace Anni
{
    public class Program
    {
        public static Config _config;
        static ChannelBot[] _botlist;
        public static Task Main(string[] arg)
        {
            _config = JsonSerializer.Deserialize<Config>(File.ReadAllText("config.json")) ?? new Config();

            //Creating Twitch Bot
            string botUsername = _config.username;
            string botOAuth = _config.token;

            ConnectionCredentials credentials = new ConnectionCredentials(botUsername, botOAuth);
            List<ChannelBot> bots = new List<ChannelBot>();
            foreach (string channel in _config.channels)
            {
                Console.WriteLine(channel);
                _ = Task.Run(async () =>
                {
                    var _bot = new ChannelBot(credentials, channel);
                    bots.Add(_bot);
                    _bot.Connect();
                });
            }
            _botlist = bots.ToArray();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
    public class Config
    {
        public string username { get; set; } = "Anni";
        public string token { get; set; } = "none";
        public string[] channels { get; set; } = new string[] {"DummyThiccBanana"};
    }

    public class ChannelBot : TwitchClient
    {
        public readonly ITwitchClient _client;
        public readonly string _channel;

        public ChannelBot(ConnectionCredentials credentials,string channel)
        {
            _channel = channel;
            // Create a new TwitchClient instance with the provided credentials
            var options = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30),
                WhispersAllowedInPeriod = 750,
                WhisperQueueCapacity = 10,
                WhisperThrottlingPeriod = TimeSpan.FromSeconds(30),
            };
            var client = new TwitchClient(new WebSocketClient(options));

            _client = client;
            _client.Initialize(credentials, channel);
            _client.OnConnected += ClientConnected;
            _client.OnDisconnected += ClientDisconnected;
            _client.OnReconnected += ClientReconnected;
            _client.OnGiftedSubscription += OnGiftedSub;
            _client.OnNewSubscriber += OnSubscribe;
            //_client.OnLog += OnLog;
            // Register event handlers for chat events
        }
        private void OnWhisper(object? sender, OnWhisperReceivedArgs e)
        {
            _client.SendWhisper(e.WhisperMessage.DisplayName, "Speak Louder.");
        }

        private void OnSubscribe(object? sender, OnNewSubscriberArgs e)
        {
            SendMessage($"Thank you {e.Subscriber.DisplayName} for subbing! Did you expect me to respond something rude?");
        }

        private void OnGiftedSub(object? sender, OnGiftedSubscriptionArgs e)
        {
            var sub = e.GiftedSubscription;
            SendMessage($"Thank you {sub.DisplayName} for the gifted sub! You pay2win..");
        }

        private void ClientReconnected(object? sender, TwitchLib.Communication.Events.OnReconnectedEventArgs e)
        {
            Console.WriteLine($"[Channel: {_channel}] Reconnected!");
        }

        private void ClientDisconnected(object? sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            Console.WriteLine($"[Channel: {_channel}] Disconnected!");
        }

        private void ClientConnected(object? sender, OnConnectedArgs e)
        {
            Console.WriteLine($"[Channel: {_channel}] Connected!");
            _client.OnMessageReceived += OnMessageRecieved;
            _client.OnWhisperReceived += OnWhisper;
        }
        public void SendMessage(string message)
        {
            _client.SendMessage(_channel, message);
        }

        private void OnMessageRecieved(object? sender, OnMessageReceivedArgs e)
        {
            Command.FindAndExecute(this,e);
        }

        private void OnLog(object? sender, OnLogArgs e)
        {
            Console.WriteLine($"[{_channel}][{e.DateTime}] {e.Data}");
        }

        public void Connect()
        {
            _client.Connect();
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }
    }
}