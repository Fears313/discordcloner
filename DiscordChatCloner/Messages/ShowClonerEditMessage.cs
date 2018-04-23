using DiscordChatCloner.Worker;

namespace DiscordChatCloner.Messages
{
    public class ShowClonerEditMessage
    {
        public ClonerWorker ClonerWorker { get; }

        public ShowClonerEditMessage(ClonerWorker clonerWorker)
        {
            ClonerWorker = clonerWorker;
        }
    }
}