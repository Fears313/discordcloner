using DiscordChatCloner.Models;
using DiscordChatCloner.Worker;

namespace DiscordChatCloner.Messages
{
    public class StopClonerMessage
    {
        public ClonerWorker ClonerWorker { get; }

        public StopClonerMessage(ClonerWorker clonerWorker) {
            ClonerWorker = clonerWorker;
        }
    }
}