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

        private Boolean _running;

        public Cloner Cloner { get; set; }
        public Guild FromGuild { get; set; }
        public Channel FromChannel { get; set; }
        public Guild ToGuild { get; set; }
        public Channel ToChannel { get; set; }
        public int PollingFrequency { get; set; }

        public bool Running
        {
            get => _running;
            set => Set(ref _running, value);
        }

        // Commands
        public RelayCommand StartClonerCommand { get; }
        public RelayCommand StopClonerCommand { get; }

        public ClonerEditViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
            StartClonerCommand = new RelayCommand(StartCloner, () => Running == false);
            StopClonerCommand = new RelayCommand(StopCloner, () => Running == true);

            // Messages
            MessengerInstance.Register<ShowClonerEditMessage>(this, m =>
            {
                Cloner = m.Cloner;
                FromGuild = Cloner.FromGuild;
                FromChannel = Cloner.FromChannel;
                ToGuild = Cloner.ToGuild;
                ToChannel = Cloner.ToChannel;
                PollingFrequency = Cloner.PollingFrequency;
                Running = Cloner.Running;
            });
        }

        private void StartCloner()
        {
            Running = true;
            MessengerInstance.Send(new StartClonerMessage(Cloner));
        }

        private void StopCloner()
        {
            Running = false;
            MessengerInstance.Send(new StopClonerMessage(Cloner));
        }
    }
}