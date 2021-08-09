using Discord;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordDotNet
{
    class Program
    {
        private readonly DiscordSocketClient _client;
        private SocketMessage MainMessage;
        private System.Timers.Timer tempTimer;

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
            _client = new DiscordSocketClient();
            _client.Log += LogAsync;
            _client.Ready += onReady;
            _client.MessageReceived += onMessage;

            tempTimer = new System.Timers.Timer(5000);
            tempTimer.Elapsed += TempTimer_Elapsed;
        }

        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, "ODczOTYxNDA4ODAxNDI3NDg2.YRAB_Q.m0eJHkqm4xoJj4A5YFap3YPda-s");
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private Task onReady()
        {
            Console.WriteLine($"{_client.CurrentUser} is Running!!");
            return Task.CompletedTask;
        }

        private async Task onMessage(SocketMessage message)
        {
            if (MainMessage == null)
            {
                MainMessage = message;
            }

            if (message.Author.Id == _client.CurrentUser.Id)
            {
                return;
            }

            if (message.Content == "!rstart")
            {
                await message.Channel.SendMessageAsync("Remindやりまーす");

                if (tempTimer.Enabled == false)
                {
                    tempTimer.Start();
                }
            }

            if (message.Content == "!rend")
            {
                await message.Channel.SendMessageAsync("Remindおわおわり");

                if (tempTimer.Enabled)
                {
                    tempTimer.Stop();
                }
            }

            if (message.Content == "!god")
            {
                await message.Channel.SendMessageAsync("神ですか？ " + message.Author.Username + "様の事です！");
            }
        }

        private async void TempTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (MainMessage != null)
            {
                await MainMessage.Channel.SendMessageAsync($"5秒ごとに送ります {DateTime.Now.ToString()}");
            }
        }
    }
}