using DiscordChatCloner.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface IClonerEditViewModel
    {
        Cloner Cloner { get; }
        Guild FromGuild { get; }
        Channel FromChannel { get; }
        Guild ToGuild { get;  }
        Channel ToChannel { get; }
        int PollingFrequency { get;  }

        //RelayCommand StartClonerCommand { get; }
        //RelayCommand StopClonerCommand { get; }
    }
}