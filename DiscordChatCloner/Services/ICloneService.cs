using System.Threading.Tasks;
using DiscordChatCloner.Models;

namespace DiscordChatCloner.Services
{
    public interface ICloneService
    {
        Task CloneAsync(string token, Channel fromoChannel, Channel toChannel, int pollingFrequency);
    }
}