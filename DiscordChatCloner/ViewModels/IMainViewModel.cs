using System.Collections.Generic;
using System.Collections.ObjectModel;
using DiscordChatCloner.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface IMainViewModel
    {
        bool IsBusy { get; }
        bool IsDataAvailable { get; }

        string Token { get; set; }

        //IReadOnlyList<Guild> AvailableGuilds { get; }
        //Guild SelectedGuild { get; set; }
        //IReadOnlyList<Channel> AvailableChannels { get; }
        ObservableCollection<Cloner> AvailableCloners { get; }

        RelayCommand PullDataCommand { get; }
        RelayCommand ShowSettingsCommand { get; }
        RelayCommand ShowAboutCommand { get; }
        //RelayCommand<Channel> ShowCloneSetupCommand { get; }
    }
}