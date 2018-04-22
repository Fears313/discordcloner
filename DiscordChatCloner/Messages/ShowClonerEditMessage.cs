using DiscordChatCloner.Models;
using System.Collections.Generic;

namespace DiscordChatCloner.Messages
{
    public class ShowClonerEditMessage
    {
        public Cloner Cloner { get; }

        public ShowClonerEditMessage(Cloner cloner)
        {
            Cloner = cloner;
        }
    }
}