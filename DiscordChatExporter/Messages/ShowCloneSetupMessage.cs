using DiscordChatExporter.Models;
using System.Collections.Generic;

namespace DiscordChatExporter.Messages
{
    public class ShowCloneSetupMessage
    {
        public Guild Guild { get; }

        public Channel Channel { get; }

        public IReadOnlyList<Channel> AvailableChannels { get; }

        public Dictionary<Guild, IReadOnlyList<Channel>> GuildChannelMap { get; }

        // TODo get rid of available channles
        public ShowCloneSetupMessage(Guild guild, Channel channel, IReadOnlyList<Channel> availableChannels, Dictionary<Guild, IReadOnlyList<Channel>> guildChannelMap)
        {
            Guild = guild;
            Channel = channel;
            AvailableChannels = availableChannels;
            GuildChannelMap = guildChannelMap;
        }
    }
}