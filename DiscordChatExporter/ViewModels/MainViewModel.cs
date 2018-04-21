using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using DiscordChatExporter.Exceptions;
using DiscordChatExporter.Messages;
using DiscordChatExporter.Models;
using DiscordChatExporter.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Tyrrrz.Extensions;

namespace DiscordChatExporter.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly IMessageGroupService _messageGroupService;
        private readonly ICloneService _cloneService;

        private  Dictionary<Guild, IReadOnlyList<Channel>> _guildChannelsMap;

        private bool _isBusy;
        private string _token;
        private IReadOnlyList<Guild> _availableGuilds;
        private Guild _selectedGuild;
        private IReadOnlyList<Channel> _availableChannels;

       
        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                PullDataCommand.RaiseCanExecuteChanged();
                ShowCloneSetupCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsDataAvailable => AvailableGuilds.NotNullAndAny();

        public string Token
        {
            get => _token;
            set
            {
                // Remove invalid chars
                value = value?.Trim('"');

                Set(ref _token, value);
                PullDataCommand.RaiseCanExecuteChanged();
            }
        }


        public Dictionary<Guild, IReadOnlyList<Channel>> GuildChannelMap
        {
            get => _guildChannelsMap;
        }

        public IReadOnlyList<Guild> AvailableGuilds
        {
            get => _availableGuilds;
            private set
            {
                Set(ref _availableGuilds, value);
                RaisePropertyChanged(() => IsDataAvailable);
            }
        }

        public Guild SelectedGuild
        {
            get => _selectedGuild;
            set
            {
                Set(ref _selectedGuild, value);
                AvailableChannels = value != null ? _guildChannelsMap[value] : new Channel[0];
                ShowCloneSetupCommand.RaiseCanExecuteChanged();
            }
        }

        public IReadOnlyList<Channel> AvailableChannels
        {
            get => _availableChannels;
            private set => Set(ref _availableChannels, value);
        }

        public RelayCommand PullDataCommand { get; }
        public RelayCommand ShowSettingsCommand { get; }
        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand<Channel> ShowExportSetupCommand { get; }
        public RelayCommand<Channel> ShowCloneSetupCommand { get; }

        public MainViewModel(ISettingsService settingsService, IDataService dataService,
            IMessageGroupService messageGroupService, ICloneService cloneService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _messageGroupService = messageGroupService;
            _cloneService = cloneService;

            _guildChannelsMap = new Dictionary<Guild, IReadOnlyList<Channel>>();

            // Commands
            PullDataCommand = new RelayCommand(PullData, () => Token.IsNotBlank() && !IsBusy);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowAboutCommand = new RelayCommand(ShowAbout);
            ShowCloneSetupCommand = new RelayCommand<Channel>(ShowCloneSetup, _ => !IsBusy);

            // Messages
            MessengerInstance.Register<StartCloneMessage>(this, m =>
            {
                DoClone(m.FromChannel, m.ToChannel, m.PollingFrequency);
            });

            // Defaults
            _token = _settingsService.LastToken;
        }


        private async void PullData()
        {
            IsBusy = true;

            // Copy token so it doesn't get mutated
            var token = Token;

            // Save token
            _settingsService.LastToken = token;

            // Clear existing
            _guildChannelsMap.Clear();

            try
            {
                // Get DM channels
                {
                    var channels = await _dataService.GetDirectMessageChannelsAsync(token);
                    var guild = new Guild("@me", "Direct Messages", null);
                    _guildChannelsMap[guild] = channels.ToArray();
                }

                // Get guild channels
                {
                    var guilds = await _dataService.GetGuildsAsync(token);
                    foreach (var guild in guilds)
                    {
                        var channels = await _dataService.GetGuildChannelsAsync(token, guild.Id);
                        _guildChannelsMap[guild] = channels.Where(c => c.Type == ChannelType.GuildTextChat).ToArray();
                    }
                }
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                //const string message = "Unauthorized to perform request. Make sure token is valid.";
                //MessengerInstance.Send(new ShowErrorMessage(message));
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                const string message = "Forbidden to perform request. The account may be locked by 2FA.";
                MessengerInstance.Send(new ShowErrorMessage(message));
            }

            AvailableGuilds = _guildChannelsMap.Keys.ToArray();
            SelectedGuild = AvailableGuilds.FirstOrDefault();
            IsBusy = false;
        }

        private void ShowSettings()
        {
            MessengerInstance.Send(new ShowSettingsMessage());
        }

        private void ShowAbout()
        {
            Process.Start("https://github.com/Tyrrrz/DiscordChatExporter");
        }

        private void ShowCloneSetup(Channel channel)
        {
            MessengerInstance.Send(new ShowCloneSetupMessage(SelectedGuild, channel, AvailableChannels, _guildChannelsMap));
        }

        private async void DoClone(Channel fromChannel, Channel toChannel, int pollingFrequency)
        {
            IsBusy = true;

            // Get last used token
            var token = _settingsService.LastToken;

            try
            {
                // Get messages
                var messages = await _dataService.GetChannelMessagesAsync(token, fromChannel.Id, null, null);

                // Group them
                var messageGroups = _messageGroupService.GroupMessages(messages);

                // Create log
                var log = new ChannelChatLog(SelectedGuild, fromChannel, messageGroups, messages.Count);


                // Clone await  We need to change things a lot from here down.  
                _cloneService.CloneAsync(token, fromChannel, toChannel, pollingFrequency);



                // Notify completion
                MessengerInstance.Send(new ShowCloneDoneMessage());
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                const string message = "Forbidden to view messages in that channel.";
                MessengerInstance.Send(new ShowErrorMessage(message));
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                const string message = "Invalid request.";
                MessengerInstance.Send(new ShowErrorMessage(message));
            }

            IsBusy = false;
        }

    }
}