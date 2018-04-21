﻿using System;
using DiscordChatExporter.Models;

namespace DiscordChatExporter.Messages
{
    public class StartCloneMessage
    {
        public Channel FromChannel { get; }

        public Channel ToChannel { get; }

        public int PollingFrequency { get; }

        public StartCloneMessage(Channel fromChannel, Channel toChannel, int pollingFrequency)
        {
            FromChannel = fromChannel;
            ToChannel = toChannel;
            PollingFrequency = pollingFrequency;
        }
    }
}