using System.ComponentModel;
using System.Threading.Tasks;
using DiscordChatCloner.Models;

namespace DiscordChatCloner.Services
{
    public interface ICloneService
    {
        Task CloneAsync(string token, Cloner cloner, BackgroundWorker worker);
        void Clone(string token, Cloner cloner, BackgroundWorker worker);
    }
}