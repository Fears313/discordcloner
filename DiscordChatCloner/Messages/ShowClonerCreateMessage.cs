using DiscordChatCloner.Models;
using System.Collections.Generic;

namespace DiscordChatCloner.Messages
{
    public class ShowClonerCreateMessage
    {
        public Dictionary<Guild, IReadOnlyList<Channel>> GuildChannelMap { get; }

        public ShowClonerCreateMessage(Dictionary<Guild, IReadOnlyList<Channel>> guildChannelMap)
        {
            GuildChannelMap = guildChannelMap;
        }
    }
}