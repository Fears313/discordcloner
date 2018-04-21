﻿using System;
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
    public class CloneSetupViewModel : ViewModelBase, ICloneSetupViewModel
    {
        private readonly ISettingsService _settingsService;

        public Guild _toGuild;
        public Channel _toChannel;
        private Dictionary<Guild, IReadOnlyList<Channel>> _guildChannelsMap;
        private int _pollingFrequency;

        public Guild Guild { get; private set; }
        public Channel Channel { get; private set; }


        public IReadOnlyList<Guild> AvailableGuilds {
            get => _guildChannelsMap.Keys.ToArray();
        }

        public IReadOnlyList<Channel> AvailableChannels {
            get => _toGuild == null ? null : _guildChannelsMap[_toGuild];
        }



        // TODO
        public Dictionary<Guild, IReadOnlyList<Channel>> GuildChannelMap
        {
            get => _guildChannelsMap;
            set => Set(ref _guildChannelsMap, value);
        }


        public Guild ToGuild
        {
            get => _toGuild;
            set => Set(ref _toGuild, value);
        }

        public Channel ToChannel
        {
            get => _toChannel;
            set => Set(ref _toChannel, value);
        }


        public int PollingFrequency
        {
            get => _pollingFrequency;
            set => Set(ref _pollingFrequency, value);
        }

        // Commands
        public RelayCommand CloneCommand { get; }

        public CloneSetupViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
            CloneCommand = new RelayCommand(Clone, () => ToChannel != null);

            // Messages
            MessengerInstance.Register<ShowCloneSetupMessage>(this, m =>
            {
                Guild = m.Guild;
                Channel = m.Channel;
                //ToGuild = m.Guild;
                GuildChannelMap = m.GuildChannelMap;
                PollingFrequency = _settingsService.PollingFrequency;
            });
        }

        private void Clone()
        {
            // Start clone
            MessengerInstance.Send(new StartCloneMessage(Channel, ToChannel, PollingFrequency));
        }
    }
}