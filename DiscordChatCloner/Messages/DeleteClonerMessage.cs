using DiscordChatCloner.Models;

namespace DiscordChatCloner.Messages
{
    public class DeleteClonerMessage
    {
        public Cloner Cloner { get; }

        public Channel ToChannel { get; }

        public int PollingFrequency { get; }

        public DeleteClonerMessage(Cloner cloner) {
            Cloner = cloner;
        }
    }
}