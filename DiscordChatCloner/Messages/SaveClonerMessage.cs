using DiscordChatCloner.Models;

namespace DiscordChatCloner.Messages
{
    public class SaveClonerMessage
    {
        public Cloner Cloner { get; }

        public SaveClonerMessage(Cloner cloner)
        {
            Cloner = cloner;
        }
    }
}