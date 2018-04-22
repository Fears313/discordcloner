using DiscordChatCloner.Models;
using System.Collections.Generic;

namespace DiscordChatCloner.Messages
{
    public class ShowCloneSetupMessage
    {
        public Guild Guild { get; }

        public Channel Channel { get; }

        public Dictionary<Guild, IReadOnlyList<Channel>> GuildChannelMap { get; }

        public ShowCloneSetupMessage(Guild guild, Channel channel, Dictionary<Guild, IReadOnlyList<Channel>> guildChannelMap)
        {
            Guild = guild;
            Channel = channel;
            GuildChannelMap = guildChannelMap;
        }
    }
}