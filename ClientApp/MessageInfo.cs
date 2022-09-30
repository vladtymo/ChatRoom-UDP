using System;

namespace ClientApp
{
    public class MessageInfo
    {
        public string? Text { get; set; }
        public DateTime Time { get; set; }

        public MessageInfo(string? text)
        {
            this.Text = text;
            Time = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Text} : {Time.ToShortTimeString()}";
        }
    }
}
