using System;
using System.Collections.Generic;
using DiscordChatExporter.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatExporter.ViewModels
{
    public interface ICloneSetupViewModel
    {
        Guild Guild { get; }
        Channel Channel { get; }
        Channel ToChannel { get; }

        RelayCommand CloneCommand { get; }
    }
}