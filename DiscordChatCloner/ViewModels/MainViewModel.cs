using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using DiscordChatCloner.Exceptions;
using DiscordChatCloner.Messages;
using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
using DiscordChatCloner.Worker;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Tyrrrz.Extensions;

namespace DiscordChatCloner.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IDataService _dataService;
        private readonly IMessageGroupService _messageGroupService;
        private readonly ICloneService _cloneService;

        private Dictionary<Guild, IReadOnlyList<Channel>> _guildChannelsMap;
        private bool _isBusy;
        private string _token;
        private bool _isDataAvailable = false;
        private ObservableCollection<ClonerWorker> _clonerWorkers;


        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                PullDataCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsDataAvailable
        {
            get => _isDataAvailable;
            private set
            {
                Set(ref _isDataAvailable, value);
                ShowClonerCreateCommand.RaiseCanExecuteChanged();
            }
        }

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

        public ObservableCollection<ClonerWorker> ClonerWorkers
        {
            get => _clonerWorkers;
            private set => Set(ref _clonerWorkers, value);
        }

        public RelayCommand PullDataCommand { get; }
        public RelayCommand ShowSettingsCommand { get; }
        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand<Channel> ShowCloneSetupCommand { get; }

        public RelayCommand ShowClonerCreateCommand { get; }
        public RelayCommand<ClonerWorker> ShowClonerEditCommand { get; }
        public RelayCommand CreateClonerCommand { get; }

        public MainViewModel(ISettingsService settingsService, IDataService dataService, IMessageGroupService messageGroupService, ICloneService cloneService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _messageGroupService = messageGroupService;
            _cloneService = cloneService;

            _guildChannelsMap = new Dictionary<Guild, IReadOnlyList<Channel>>();
            ClonerWorkers = new ObservableCollection<ClonerWorker>();

            // Commands
            PullDataCommand = new RelayCommand(PullData, () => Token.IsNotBlank() && !IsBusy);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowAboutCommand = new RelayCommand(ShowAbout);
            ShowClonerCreateCommand = new RelayCommand(ShowClonerCreate, () => IsDataAvailable);
            ShowClonerEditCommand = new RelayCommand<ClonerWorker>(ShowClonerEdit, _ => !IsBusy);

            MessengerInstance.Register<CreateClonerMessage>(this, m => {
                CreateCloner(m.Name, m.FromGuild, m.FromChannel, m.ToGuild, m.ToChannel, m.PollingFrequency);
            });

            //MessengerInstance.Register<StartClonerMessage>(this, m => {
            //    DoClone(m.Cloner);
            //});

            // Defaults
            _token = _settingsService.LastToken;
            var cloners = _settingsService.Cloners;
            foreach(var cloner in cloners)
            {
                ClonerWorkers.Add(new ClonerWorker(cloner, Token, _cloneService));
            }
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

                IsDataAvailable = true;
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                const string message = "Unauthorized to perform request. Make sure token is valid.";
                MessengerInstance.Send(new ShowErrorMessage(message));
            }
            catch (HttpErrorStatusCodeException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                const string message = "Forbidden to perform request. The account may be locked by 2FA.";
                MessengerInstance.Send(new ShowErrorMessage(message));
            }

            IsBusy = false;
        }

        private void ShowSettings()
        {
            MessengerInstance.Send(new ShowSettingsMessage());
        }

        private void ShowAbout()
        {
            Process.Start("https://github.com/Tyrrrz/DiscordChatCloner");
        }

        private void ShowClonerCreate()
        {
            MessengerInstance.Send(new ShowClonerCreateMessage(_guildChannelsMap));
        }

        private void ShowClonerEdit(ClonerWorker clonerWorker)
        {
            MessengerInstance.Send(new ShowClonerEditMessage(clonerWorker));
        }

        private void CreateCloner(string name, Guild fromGuild, Channel fromChannel, Guild toGuild, Channel toChannel, int pollingFrequency)
        {
            Cloner cloner = new Cloner("new", name, fromGuild, fromChannel, toGuild, toChannel, pollingFrequency);
            ClonerWorkers.Add(new ClonerWorker(cloner, Token, _cloneService));
            _settingsService.Cloners.Add(cloner);
        }
    }
}