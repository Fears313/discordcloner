using DiscordChatCloner.Models;
using DiscordChatCloner.Services;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordChatCloner.Worker
{
    public class ClonerWorker
    {
        public Cloner Cloner { get; private set; }
        public bool IsRunning { get; private set; }

        private readonly string _token;
        private readonly ICloneService _cloneService;
        private Task<Timer> clonerTask;

        public ClonerWorker(Cloner cloner, string token, ICloneService cloneService) {
            Cloner = cloner;
            IsRunning = false;
            _token = token;
            _cloneService = cloneService;
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
    }
}
