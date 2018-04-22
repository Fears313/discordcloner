using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DiscordChatCloner.Exceptions;
using DiscordChatCloner.Messages;
using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
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

        private  Dictionary<Guild, IReadOnlyList<Channel>> _guildChannelsMap;

        private bool _isBusy;
        private string _token;
        //private IReadOnlyList<Guild> _availableGuilds;
        //private Guild _selectedGuild;
        //private IReadOnlyList<Channel> _availableChannels;
        private ObservableCollection<Cloner> _availableCloners;


        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(ref _isBusy, value);
                PullDataCommand.RaiseCanExecuteChanged();
                //ShowCloneSetupCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsDataAvailable => true;

        //public bool IsDataAvailable => _guildChannelsMap.NotNullAndAny();

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

        //public IReadOnlyList<Guild> AvailableGuilds
        //{
        //    get => _availableGuilds;
        //    private set
        //    {
        //        Set(ref _availableGuilds, value);
        //        RaisePropertyChanged(() => IsDataAvailable);
        //    }
        //}

        //public Guild SelectedGuild
        //{
        //    get => _selectedGuild;
        //    set
        //    {
        //        Set(ref _selectedGuild, value);
        //        AvailableChannels = value != null ? _guildChannelsMap[value] : new Channel[0];
        //        ShowCloneSetupCommand.RaiseCanExecuteChanged();
        //    }
        //}

        //public IReadOnlyList<Channel> AvailableChannels
        //{
        //    get => _availableChannels;
        //    private set => Set(ref _availableChannels, value);
        //}

        public ObservableCollection<Cloner> AvailableCloners
        {
            get => _availableCloners;
            private set => Set(ref _availableCloners, value);
        }

        public RelayCommand PullDataCommand { get; }
        public RelayCommand ShowSettingsCommand { get; }
        public RelayCommand ShowAboutCommand { get; }
        public RelayCommand<Channel> ShowCloneSetupCommand { get; }

        public RelayCommand ShowClonerCreateCommand { get; }
        public RelayCommand<Cloner> ShowClonerEditCommand { get; }

        public RelayCommand CreateClonerCommand { get; }

        public MainViewModel(ISettingsService settingsService, IDataService dataService,
            IMessageGroupService messageGroupService, ICloneService cloneService)
        {
            _settingsService = settingsService;
            _dataService = dataService;
            _messageGroupService = messageGroupService;
            _cloneService = cloneService;

            _guildChannelsMap = new Dictionary<Guild, IReadOnlyList<Channel>>();
            AvailableCloners = new ObservableCollection<Cloner>();

            // Commands
            PullDataCommand = new RelayCommand(PullData, () => Token.IsNotBlank() && !IsBusy);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowAboutCommand = new RelayCommand(ShowAbout);
            //ShowCloneSetupCommand = new RelayCommand<Channel>(ShowCloneSetup, _ => !IsBusy);
            ShowClonerCreateCommand = new RelayCommand(ShowClonerCreate);
            ShowClonerEditCommand = new RelayCommand<Cloner>(ShowClonerEdit);

            MessengerInstance.Register<CreateClonerMessage>(this, m => {
                CreateCloner(m.Name, m.FromGuild, m.FromChannel, m.ToGuild, m.ToChannel, m.PollingFrequency);
            });

            // Messages
            //MessengerInstance.Register<StartCloneMessage>(this, m => {
            //    DoClone(m.FromChannel, m.ToChannel, m.PollingFrequency);
            //});

            // Messages
            MessengerInstance.Register<StartClonerMessage>(this, m => {
                DoClone(m.Cloner);
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

            //AvailableGuilds = _guildChannelsMap.Keys.ToArray();
            //SelectedGuild = AvailableGuilds.FirstOrDefault();
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

        //private void ShowCloneSetup(Channel channel)
        //{
        //    MessengerInstance.Send(new ShowCloneSetupMessage(SelectedGuild, channel, _guildChannelsMap));
        //}

        private void ShowClonerCreate()
        {
            MessengerInstance.Send(new ShowClonerCreateMessage(_guildChannelsMap));
        }

        private void ShowClonerEdit(Cloner cloner)
        {
            MessengerInstance.Send(new ShowClonerEditMessage(cloner));
        }

        private async void CreateCloner(string name, Guild fromGuild, Channel fromChannel, Guild toGuild, Channel toChannel, int pollingFrequency)
        {
            Cloner cloner = new Cloner("new", name, fromGuild, fromChannel, toGuild, toChannel, pollingFrequency);
            this.AvailableCloners.Add(cloner);
        }


        private async void DoClone(Cloner cloner)
        {
            IsBusy = true;

            // Get last used token
            var token = _settingsService.LastToken;

            try
            {
                // Get messages
                var messages = await _dataService.GetChannelMessagesAsync(token, cloner.FromChannel.Id, null, null);

                // Group them
                var messageGroups = _messageGroupService.GroupMessages(messages);

                // Create log
                //var log = new ChannelChatLog(SelectedGuild, fromChannel, messageGroups, messages.Count);
                CreateWorker();

                IDictionary<string, object> context = new Dictionary<string, object>();
                context.Add("token", token);
                context.Add("cloner", cloner);

                cloneWorker.RunWorkerAsync(context);

                // Clone await  We need to change things a lot from here down.  
                //await _cloneService.CloneAsync(token, cloner, cloneWorker);

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

        private BackgroundWorker cloneWorker;

        private async void StartCloningAsync(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = (BackgroundWorker) sender;
            IDictionary<string, object> context = (IDictionary<string, object>)e.Argument;
            string token = (string)context["token"];
            Cloner cloner = (Cloner)context["cloner"];
            bgWorker.ReportProgress(0, "Starting cloner: " + cloner.Name);

            //await _cloneService.CloneAsync(token, cloner, bgWorker);
            _cloneService.Clone(token, cloner, bgWorker);

            bgWorker.ReportProgress(100, "Finishing - " + cloner.Name);
        }


        private void CreateWorker()
        {
            cloneWorker = new BackgroundWorker();
            cloneWorker.WorkerReportsProgress = true;
            cloneWorker.DoWork += StartCloningAsync;
            cloneWorker.ProgressChanged += worker_ProgressChanged;
            cloneWorker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("I have an update --- things are ok! - " + e.ProgressPercentage.ToString() + " and " + e.UserState);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("We are done here.  I tink we'll move on!!!");
        }
    }
}