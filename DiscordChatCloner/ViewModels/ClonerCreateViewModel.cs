using System.Collections.Generic;
using System.Linq;
using DiscordChatCloner.Messages;
using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public class ClonerCreateViewModel : ViewModelBase, IClonerCreateViewModel
    {
        private readonly ISettingsService _settingsService;

        public string _name;
        public Guild _fromGuild;
        public Channel _fromChannel;
        public Guild _toGuild;
        public Channel _toChannel;
        public Dictionary<Guild, IReadOnlyList<Channel>> _guildChannelMap;
        public int _pollingFrequency;
        private IReadOnlyList<Channel> _availableFromChannels;
        private IReadOnlyList<Channel> _availableToChannels;

        public IReadOnlyList<Guild> AvailableGuilds {
            get => _guildChannelMap.Keys.ToArray();
        }

        public IReadOnlyList<Channel> AvailableFromChannels
        {
            get => _availableFromChannels;
            set => Set(ref _availableFromChannels, value);
        }

        public IReadOnlyList<Channel> AvailableToChannels
        {
            get => _availableToChannels;
            set => Set(ref _availableToChannels, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public Guild FromGuild
        {
            get => _fromGuild;
            set
            {
                Set(ref _fromGuild, value);
                AvailableFromChannels = _fromGuild == null ? null : _guildChannelMap[_fromGuild];
            }
        }

        public Channel FromChannel
        {
            get => _fromChannel;
            set => Set(ref _fromChannel, value);
        }

        public Guild ToGuild
        {
            get => _toGuild;
            set {
                Set(ref _toGuild, value);
                AvailableToChannels = _toGuild == null ? null : _guildChannelMap[_toGuild];
            }
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
        public RelayCommand CreateClonerCommand { get; }

        public ClonerCreateViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
            CreateClonerCommand = new RelayCommand(CreateCloner, () => FromChannel != null && ToChannel != null && PollingFrequency > 0);

            // Messages
            MessengerInstance.Register<ShowClonerCreateMessage>(this, m =>
            {
                _guildChannelMap = m.GuildChannelMap;
                PollingFrequency = _settingsService.PollingFrequency;
            });
        }

        private void CreateCloner()
        {
            MessengerInstance.Send(new CreateClonerMessage(Name, FromGuild, FromChannel, ToGuild, ToChannel, PollingFrequency));
        }
    }
}