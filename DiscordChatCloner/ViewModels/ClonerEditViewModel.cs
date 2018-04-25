using DiscordChatCloner.Messages;
using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
using DiscordChatCloner.Worker;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public class ClonerEditViewModel : ViewModelBase, IClonerEditViewModel
    {
        private readonly ISettingsService _settingsService;

        public ClonerWorker ClonerWorker { get; private set; }

        public Cloner Cloner { get; set; }
        public Guild FromGuild { get; set; }
        public Channel FromChannel { get; set; }
        public Guild ToGuild { get; set; }
        public Channel ToChannel { get; set; }
        public int PollingFrequency { get; set; }
        public bool IsRunning { get => ClonerWorker.IsRunning; }

        // Commands
        public RelayCommand StartClonerCommand { get; }
        public RelayCommand StopClonerCommand { get; }
        public RelayCommand DeleteClonerCommand { get; }

        public ClonerEditViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            // Commands
            StartClonerCommand = new RelayCommand(StartCloner, () => IsRunning == false);
            StopClonerCommand = new RelayCommand(StopCloner, () => IsRunning == true);
            DeleteClonerCommand = new RelayCommand(DeleteCloner, () => IsRunning == false);

            // Messages
            MessengerInstance.Register<ShowClonerEditMessage>(this, m =>
            {
                ClonerWorker = m.ClonerWorker;
                Cloner = m.ClonerWorker.Cloner;
                FromGuild = Cloner.FromGuild;
                FromChannel = Cloner.FromChannel;
                ToGuild = Cloner.ToGuild;
                ToChannel = Cloner.ToChannel;
                PollingFrequency = Cloner.PollingFrequency;
            });
        }

        private void StartCloner()
        {
            MessengerInstance.Send(new StartClonerMessage(ClonerWorker));
        }

        private void StopCloner()
        {
            MessengerInstance.Send(new StopClonerMessage(ClonerWorker));
        }

        private void DeleteCloner()
        {
            MessengerInstance.Send(new DeleteClonerMessage(Cloner));
        }
    }
}