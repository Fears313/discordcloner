using System;
using DiscordChatExporter.Models;

namespace DiscordChatExporter.Messages
{
    public class StartCloneMessage
    {
        public Channel FromChannel { get; }

        public Channel ToChannel { get; }

        public Channel Channel { get; }

        public ExportFormat Format { get; }

        public DateTime? From { get; }

        public DateTime? To { get; }

        public StartCloneMessage(Channel channel, ExportFormat format, DateTime? from, DateTime? to, Channel fromChannel, Channel toChannel)
        {
            Channel = channel;
            FromChannel = fromChannel;
            ToChannel = toChannel;
            Format = format;
            From = from;
            To = to;
        }
    }
}