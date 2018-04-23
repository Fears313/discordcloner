using System.Threading.Tasks;
using System.Timers;
using DiscordChatCloner.Models;

namespace DiscordChatCloner.Services
{
    public interface ICloneService
    {
        Task<Timer> CloneAsync(string token, Cloner cloner);
    }
}