namespace DiscordChatCloner.ViewModels
{
    public interface ISettingsViewModel
    {
        string DateFormat { get; set; }
        int MessageGroupLimit { get; set; }
    }
}