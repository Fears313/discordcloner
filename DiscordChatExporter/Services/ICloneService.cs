using System.Threading.Tasks;
using DiscordChatExporter.Models;

namespace DiscordChatExporter.Services
{
    public interface ICloneService
    {
        Task CloneAsync(string token, Channel fromoChannel, Channel toChannel, int pollingFrequency);
    }
}