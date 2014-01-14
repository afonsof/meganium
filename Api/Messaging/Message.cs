namespace Meganium.Api.Messaging
{
    public class Message
    {
        public string Text { get; set; }
        public MessageType Type { get; set; }

        public Message(string text, MessageType type)
        {
            Text = text;
            Type = type;
        }
    }
}