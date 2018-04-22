using DiscordChatCloner.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface ICloneSetupViewModel
    {
        Guild Guild { get; }
        Channel Channel { get; }
        Channel ToChannel { get; }

        RelayCommand CloneCommand { get; }
    }
}