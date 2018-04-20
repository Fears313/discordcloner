using System;
using DiscordChatExporter.Models;

namespace DiscordChatExporter.Messages
{
    public class StartCloneMessage
    {
        public Channel FromChannel { get; }

        public Channel ToChannel { get; }

        public StartCloneMessage(Channel fromChannel, Channel toChannel)
        {
            FromChannel = fromChannel;
            ToChannel = toChannel;
        }
    }
}