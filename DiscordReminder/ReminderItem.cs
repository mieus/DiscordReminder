using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordReminder
{
    public class ReminderItem
    {
        public string Key { get; set; }

        public DateTime Time { get; set; }

        public string Comment { get; set; }

        /// <summary>
        /// 曜日
        /// 0 1 2 3 4 5 6
        /// 日月火水木金土
        /// </summary>
        public bool[] IsWeek { get; set; } = new bool[7];

        public bool IsEveryone { get; set; }

        public bool IsRemind { get; set; }

        public ReminderItem()
        {
            // フォーマットは「キー 時間 コメント 曜日」
        }

        public string GetWeekText()
        {
            var text = IsWeek[0] ? "日" : string.Empty;
            text += IsWeek[1] ? "月" : string.Empty;
            text += IsWeek[2] ? "火" : string.Empty;
            text += IsWeek[3] ? "水" : string.Empty;
            text += IsWeek[4] ? "木" : string.Empty;
            text += IsWeek[5] ? "金" : string.Empty;
            text += IsWeek[6] ? "土" : string.Empty;
            return text;
        }

        public static ReminderItem Create(string value)
        {
            string[] values = value.Split(' ');

            if (values.Length >= 5)
            {
                var key = values[0];
                var time = DateTime.Parse(values[1]);
                var comment = values[2];
                var isEveryone = values[3] == "y" ? true : false;
                bool[] isWeeks = new bool[7];

                if (values[4] == "all")
                {
                    isWeeks[0] = true;
                    isWeeks[1] = true;
                    isWeeks[2] = true;
                    isWeeks[3] = true;
                    isWeeks[4] = true;
                    isWeeks[5] = true;
                    isWeeks[6] = true;
                }
                else
                {
                    isWeeks[0] = values[4].IndexOf("日") != -1 ? true : false;
                    isWeeks[1] = values[4].IndexOf("月") != -1 ? true : false;
                    isWeeks[2] = values[4].IndexOf("火") != -1 ? true : false;
                    isWeeks[3] = values[4].IndexOf("水") != -1 ? true : false;
                    isWeeks[4] = values[4].IndexOf("木") != -1 ? true : false;
                    isWeeks[5] = values[4].IndexOf("金") != -1 ? true : false;
                    isWeeks[6] = values[4].IndexOf("土") != -1 ? true : false;
                }

                return new ReminderItem() { Key = key, Time = time, Comment = comment, IsEveryone = isEveryone, IsWeek = isWeeks };
            }
            else
            {
                new Exception("引数が違います !rhelpをお読みください");
            }

            return null;
        }
    }
}
