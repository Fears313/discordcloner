using DiscordChatCloner.Models;

namespace DiscordChatCloner.Messages
{
    public class StartClonerMessage
    {
        public Cloner Cloner { get; }

        public Channel ToChannel { get; }

        public int PollingFrequency { get; }

        public StartClonerMessage(Cloner cloner) {
            Cloner = cloner;
        }
    }
}