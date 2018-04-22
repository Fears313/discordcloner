using System;
using DiscordChatCloner.Models;

namespace DiscordChatCloner.Messages
{
    public class CreateClonerMessage
    {
        public string Name { get; }
        public Guild FromGuild { get; }
        public Channel FromChannel { get; }
        public Guild ToGuild { get; }
        public Channel ToChannel { get; }
        public int PollingFrequency { get; }

        public CreateClonerMessage(string name, Guild fromGuild, Channel fromChannel, Guild toGuild, Channel toChannel, int pollingFrequency)
        {
            Name = name;
            FromGuild = fromGuild;
            FromChannel = fromChannel;
            ToGuild = toGuild;
            ToChannel = toChannel;
            PollingFrequency = pollingFrequency;
        }
    }
}