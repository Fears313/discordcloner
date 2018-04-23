using System.Collections.Generic;
using System.Collections.ObjectModel;
using DiscordChatCloner.Models;
using DiscordChatCloner.Worker;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface IMainViewModel
    {
        bool IsBusy { get; }
        bool IsDataAvailable { get; }

        string Token { get; set; }

        ObservableCollection<ClonerWorker> ClonerWorkers { get; }

        RelayCommand PullDataCommand { get; }
        RelayCommand ShowSettingsCommand { get; }
        RelayCommand ShowAboutCommand { get; }
    }
}