using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordChatExporter.Models;

namespace DiscordChatExporter.Services
{
    public interface IDataService
    {
        Task<IReadOnlyList<Guild>> GetGuildsAsync(string token);

        Task<IReadOnlyList<Channel>> GetDirectMessageChannelsAsync(string token);

        Task<IReadOnlyList<Channel>> GetGuildChannelsAsync(string token, string guildId);

        Task<IReadOnlyList<Message>> GetChannelMessagesAsync(string token, string channelId,
            DateTime? from, DateTime? to);

        Task<IReadOnlyList<Message>> GetChannelMessagesAsync(string token, string channelId, string messageId);


        Task<Object> PublishMessageAsync(string token, string channelId, Message message);
        Task<Object> PublishStringAsync(string token, string channelId, string message);
    }
}