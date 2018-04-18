using DiscordChatExporter.Models;

namespace DiscordChatExporter.Messages
{
    public class ShowCloneSetupMessage
    {
        public Guild Guild { get; }

        public Channel Channel { get; }

        public ShowCloneSetupMessage(Guild guild, Channel channel)
        {
            Guild = guild;
            Channel = channel;
        }
    }
}