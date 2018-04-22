using DiscordChatCloner.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface IClonerCreateViewModel
    {
        Guild FromGuild { get; }
        Channel FromChannel { get; }
        Guild ToGuild { get; }
        Channel ToChannel { get; }
        int PollingFrequency { get; }

        RelayCommand CreateClonerCommand { get; }
    }
}