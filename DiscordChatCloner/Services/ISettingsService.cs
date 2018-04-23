using DiscordChatCloner.Models;
using System.Collections.Generic;

namespace DiscordChatCloner.Services
{
    public interface ISettingsService
    {
        string DateFormat { get; set; }
        int MessageGroupLimit { get; set; }

        string LastToken { get; set; }
        int PollingFrequency { get; set; }

        List<Cloner> Cloners { get; set; }

        void Load();
        void Save();
    }
}