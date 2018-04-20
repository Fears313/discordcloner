using DiscordChatExporter.Models;
using System.Collections.Generic;

namespace DiscordChatExporter.Messages
{
    public class ShowCloneSetupMessage
    {
        public Guild Guild { get; }

        public Channel Channel { get; }

        public IReadOnlyList<Channel> AvailableChannels { get; }

        public ShowCloneSetupMessage(Guild guild, Channel channel, IReadOnlyList<Channel> availableChannels)
        {
            Guild = guild;
            Channel = channel;
            AvailableChannels = availableChannels;
        }
    }
}