using System;

namespace DiscordChatCloner.Models
{
    public static class Extensions
    {
        public static string GetChannelName(this Channel channel)
        {
            return channel.Name;
        }

        public static string GetGuildName(this Guild guild)
        {
            return guild.Name;
        }

    }
}