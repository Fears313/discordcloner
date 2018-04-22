using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordChatCloner.Messages;
using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Tyrrrz.Extensions;

namespace DiscordChatCloner.ViewModels
{
    public class ClonerEditViewModel : ViewModelBase, IClonerEditViewModel
    {
        private readonly ISettingsService _settingsService;

        public Guild FromGuild { get; set; }
        public Channel FromChannel { get; set; }

        public Guild ToGuild { get; set; }
        public Channel ToChannel { get; set; }

        public int PollingFrequency { get; set; }

        public bool Status { get; set; }

        // Commands
        public RelayCommand CloneCommand { get; }
        public RelayCommand StartClonerCommand { get; }
        public RelayCommand StopClonerCommand { get; }

        public ClonerEditViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
            CloneCommand = new RelayCommand(Clone, () => ToChannel != null);
            StartClonerCommand = new RelayCommand(StartCloner, () => Status == false);
            StopClonerCommand = new RelayCommand(StopCloner, () => Status == true);

            // Messages
            MessengerInstance.Register<ShowClonerEditMessage>(this, m =>
            {
                FromGuild = m.Cloner.FromGuild;
                FromChannel = m.Cloner.FromChannel;
                ToGuild = m.Cloner.ToGuild;
                ToChannel = m.Cloner.ToChannel;
                PollingFrequency = m.Cloner.PollingFrequency;
                Status = m.Cloner.Running;
            });
        }

        private void StartCloner()
        {
            Status = true;
        }

        private void StopCloner()
        {
            Status = false;
        }

        private void Clone()
        {
            // Start clone
            MessengerInstance.Send(new StartCloneMessage(FromChannel, ToChannel, PollingFrequency));
        }
    }
}