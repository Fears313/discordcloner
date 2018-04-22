using DiscordChatCloner.Models;

namespace DiscordChatCloner.Messages
{
    public class StopClonerMessage
    {
        public Cloner Cloner { get; }

        public Channel ToChannel { get; }

        public int PollingFrequency { get; }

        public StopClonerMessage(Cloner cloner) {
            Cloner = cloner;
        }
    }
}