using DiscordChatCloner.Messages;
using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordChatCloner.Worker
{
    public class ClonerWorker : ViewModelBase
    {
        public Cloner Cloner { get; private set; }
        public bool IsRunning { get; private set; }

        private readonly string _token;
        private readonly ICloneService _cloneService;
        private Task<Timer> clonerTask;


        // Commands
        public RelayCommand StartClonerCommand { get; }
        public RelayCommand StopClonerCommand { get; }
        public RelayCommand DeleteClonerCommand { get; }

        public ClonerWorker(Cloner cloner, string token, ICloneService cloneService) {
            Cloner = cloner;
            IsRunning = false;
            _token = token;
            _cloneService = cloneService;

            // Commands
            StartClonerCommand = new RelayCommand(StartCloner, () => IsRunning == false);
            StopClonerCommand = new RelayCommand(StopCloner, () => IsRunning == true);
            DeleteClonerCommand = new RelayCommand(DeleteCloner, () => IsRunning == false);

        }

        public void Start() {
            clonerTask = _cloneService.CloneAsync(_token, Cloner);
            IsRunning = true;
        }

        public async void Stop() {
            Timer timer = await clonerTask;
            timer.Enabled = false;
            IsRunning = false;
        }

        public override string ToString() {
            return Cloner.Name;
        }

        private void StartCloner()
        {
            MessengerInstance.Send(new StartClonerMessage(this));
        }

        private void StopCloner()
        {
            MessengerInstance.Send(new StopClonerMessage(this));
        }

        private void DeleteCloner()
        {
            MessengerInstance.Send(new DeleteClonerMessage(Cloner));
        }
    }
}
