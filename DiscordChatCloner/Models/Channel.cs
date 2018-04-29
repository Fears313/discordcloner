namespace DiscordChatCloner.Models
{
    public class Channel
    {
        public string Id { get; }

        public string Name { get; }

        public ChannelType Type { get; }

        public string LastMessageId { get; }

        public Channel(string id, string name, ChannelType type, string lastMessageId)
        {
            Id = id;
            Name = name;
            Type = type;
            LastMessageId = lastMessageId;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}