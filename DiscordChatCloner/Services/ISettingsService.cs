using DiscordChatCloner.Models;

namespace DiscordChatCloner.Services
{
    public interface ISettingsService
    {
        string DateFormat { get; set; }
        int MessageGroupLimit { get; set; }

        string LastToken { get; set; }
        int PollingFrequency { get; set; }

        void Load();
        void Save();
    }
}