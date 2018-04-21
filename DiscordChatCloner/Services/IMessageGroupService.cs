using System.Collections.Generic;
using DiscordChatCloner.Models;

namespace DiscordChatCloner.Services
{
    public interface IMessageGroupService
    {
        IReadOnlyList<MessageGroup> GroupMessages(IReadOnlyList<Message> messages);
    }
}