using DiscordChatCloner.Models;
using System.Collections.Generic;
using Tyrrrz.Settings;

namespace DiscordChatCloner.Services
{
    public class SettingsService : SettingsManager, ISettingsService
    {
        public string DateFormat { get; set; } = "dd-MMM-yy hh:mm tt";
        public int MessageGroupLimit { get; set; } = 20;

        public string LastToken { get; set; }

        public int PollingFrequency { get; set; } = 20000;

        public List<Cloner> Cloners { get; set; } = new List<Cloner>();

        public SettingsService()
        {
            Configuration.StorageSpace = StorageSpace.Instance;
            Configuration.SubDirectoryPath = "";
            Configuration.FileName = "Settings.dat";
        }
    }
}