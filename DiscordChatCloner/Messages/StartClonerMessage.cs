using DiscordChatCloner.Worker;

namespace DiscordChatCloner.Messages
{
    public class StartClonerMessage
    {
        public ClonerWorker ClonerWorker { get; }

        public StartClonerMessage(ClonerWorker clonerWorker) {
            ClonerWorker = clonerWorker;
        }
    }
}