﻿using System;
using DiscordChatExporter.Models;

namespace DiscordChatExporter.Messages
{
    public class StartCloneMessage
    {
        public Channel Channel { get; }

        public string FilePath { get; }

        public ExportFormat Format { get; }

        public DateTime? From { get; }

        public DateTime? To { get; }

        public StartCloneMessage(Channel channel, string filePath, ExportFormat format,
            DateTime? from, DateTime? to)
        {
            Channel = channel;
            FilePath = filePath;
            Format = format;
            From = from;
            To = to;
        }
    }
}