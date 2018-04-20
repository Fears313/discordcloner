using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordChatExporter.Messages;
using DiscordChatExporter.Models;
using DiscordChatExporter.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Tyrrrz.Extensions;

namespace DiscordChatExporter.ViewModels
{
    public class CloneSetupViewModel : ViewModelBase, ICloneSetupViewModel
    {
        private readonly ISettingsService _settingsService;

        public Guild _toGuild;
        public Channel _toChannel;
        private Dictionary<Guild, IReadOnlyList<Channel>> _guildChannelsMap;

        public Guild Guild { get; private set; }
        public Channel Channel { get; private set; }


        public IReadOnlyList<Guild> AvailableGuilds {
            get => _guildChannelsMap.Keys.ToArray();
        }

        public IReadOnlyList<Channel> AvailableChannels {
            get => _guildChannelsMap[_toGuild];
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

        // Commands
        public RelayCommand CloneCommand { get; }

        public CloneSetupViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
//            CloneCommand = new RelayCommand(Clone, () => FilePath.IsNotBlank());
            CloneCommand = new RelayCommand(Clone);

            // Messages
            MessengerInstance.Register<ShowCloneSetupMessage>(this, m =>
            {
                Guild = m.Guild;
                Channel = m.Channel;
                ToGuild = m.Guild;
                GuildChannelMap = m.GuildChannelMap;
            });
        }

        private void Clone()
        {
            // Start clone
            MessengerInstance.Send(new StartCloneMessage(Channel, ToChannel));
        }
    }
}