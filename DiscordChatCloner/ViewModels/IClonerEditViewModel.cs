using System;
using System.Collections.Generic;
using DiscordChatCloner.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface IClonerEditViewModel
    {
        Guild FromGuild { get;  }
        Channel FromChannel { get; }
        Guild ToGuild { get;  }
        Channel ToChannel { get; }
        int PollingFrequency { get;  }

        RelayCommand CloneCommand { get; }
    }
}