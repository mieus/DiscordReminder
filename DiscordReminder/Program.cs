using Discord;
using Discord.WebSocket;
using DiscordReminder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordDotNet
{
    public class Program
    {
        private readonly DiscordSocketClient _client;
        private SocketMessage MainMessage;
        private System.Timers.Timer tempTimer;
        private List<ReminderItem> items = new List<ReminderItem>();


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

            Load();

            tempTimer = new System.Timers.Timer(5000);
            tempTimer.Elapsed += TempTimer_Elapsed;

            CreateRemiderItems();
        }

        private void CreateRemiderItems()
        {
            ////var item1 = new ReminderItem() { Key = "Test",
            ////                                 IsEveryone = true,
            ////                                 Time = new DateTime(2000,1,1,22,55,0),
            ////                                 Comment = "テストだよ",
            ////                                 IsWeek = new bool[7] { true, true, true, true, true, true, true,}
            ////};

            ////this.items.Add(item1);
        }


        public async Task MainAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Settings.Default.token);
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
            if (message.Author.Id == _client.CurrentUser.Id)
            {
                return;
            }

            if (message.Content == "!rhelp")
            {
                await message.Channel.SendMessageAsync(Settings.Default.heip);
            }

            if (message.Content == "!rstart")
            {
                MainMessage = message;

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

            if (message.Content.IndexOf("!raddList") != -1)
            {
                string[] messageList = message.Content.Split("\n");
                foreach (var mess in messageList)
                {
                    if (mess == "!raddList")
                    {
                        continue;
                    }

                    try
                    {
                        string text = string.Empty;
                        var item = ReminderItem.Create(mess.Replace("!radd ", string.Empty));
                        if (item != null)
                        {
                            items.Add(item);
                            text += $"KEY:{item.Key} を登録しました！" + Environment.NewLine;
                        }

                        await message.Channel.SendMessageAsync(text);
                    }
                    catch (Exception err)
                    {
                        await message.Channel.SendMessageAsync(err.Message);
                    }
                }

                Save();

                return;
            }

            if (message.Content.IndexOf("!radd ") != -1)
            {
                try
                {
                    var item = ReminderItem.Create(message.Content.Replace("!radd ", string.Empty));
                    if (item != null)
                    {
                        items.Add(item);
                        await message.Channel.SendMessageAsync($"KEY:{item.Key} を登録しました！");
                    }
                }
                catch(Exception err)
                {
                    await message.Channel.SendMessageAsync(err.Message);
                }

                Save();

                return;
            }

            if (message.Content.IndexOf("!rdelete ") != -1)
            {
                var key = message.Content.Replace("!rdelete ", string.Empty);
                var tempItems = items.Where(o => o.Key == key);
                if (tempItems.Count() != 0)
                {
                    foreach (var item2 in tempItems)
                    {
                        items.Remove(item2);
                        await message.Channel.SendMessageAsync($"KEY:{item2.Comment}");
                    }
                }
                else
                {
                    await message.Channel.SendMessageAsync("そんなキーありません !rlistで確認してください");
                }

                Save();

                return;
            }

            if (message.Content == "!rlist")
            {
                if (items.Count == 0)
                {
                    await message.Channel.SendMessageAsync("何も登録されていません");
                    return;
                }

                var text = string.Empty;
                foreach (var item in items)
                {
                    text += $"KEY:{item.Key} 時間:{item.Time.ToShortTimeString()} 曜日:{item.GetWeekText()} everyone:{item.IsEveryone} コメント:{item.Comment} 通知済:{item.IsRemind}";
                    text += Environment.NewLine;
                }

                await message.Channel.SendMessageAsync(text);

                return;
            }

            if (message.Content == "!rclear")
            {
                items.Clear();
                Save();
                await message.Channel.SendMessageAsync("リマインドリストをクリアしました");

                return;
            }
        }

        private async void TempTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (MainMessage == null)
            {
                return;
            }

            // 曜日のindexを取得
            int weekIndex = (int)DateTime.Today.DayOfWeek;

            // 現在時刻取得（一応頭で取る）
            var nowTime = DateTime.Now;

            foreach (var item in items)
            {
                // 曜日一致
                if (item.IsWeek[weekIndex] == false)
                {
                    continue;
                }

                // 通知済み？
                if (item.IsRemind)
                {
                    continue;
                }

                // 通知済みフラグを落とす？1H以上経過
                if (item.Time.Hour != nowTime.Hour)
                {
                    item.IsRemind = false;
                    continue;
                }

                // 時間の分が一致
                if (item.Time.Hour == nowTime.Hour &&
                    item.Time.Minute == nowTime.Minute)
                {
                    // 通知
                    item.IsRemind = true;

                    string message = string.Empty;

                    if (item.IsEveryone)
                    {
                        message += "@everyone" + Environment.NewLine;
                    }

                    message += item.Comment;

                    await MainMessage.Channel.SendMessageAsync(message);
                }
            }
        }

        private void Save()
        {
            string fileName = @"RemindList.xml";

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<ReminderItem>));

            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            //シリアル化し、XMLファイルに保存する
            serializer.Serialize(sw, items);

            //ファイルを閉じる
            sw.Close();
        }

        private void Load()
        {
            try
            {
                //保存元のファイル名
                string fileName = "RemindList.xml";

                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<ReminderItem>));
                //読み込むファイルを開く
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    fileName, new System.Text.UTF8Encoding(false));
                //XMLファイルから読み込み、逆シリアル化する
                items = (List<ReminderItem>)serializer.Deserialize(sr);
                //ファイルを閉じる
                sr.Close();
            }
            catch
            {

            }
        }
    }
}