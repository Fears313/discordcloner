using DiscordChatCloner.Messages;
using GalaSoft.MvvmLight;

namespace DiscordChatCloner.ViewModels
{
    public class ErrorViewModel : ViewModelBase, IErrorViewModel
    {
        public string Message { get; private set; }

        public ErrorViewModel()
        {
            // Messages
            MessengerInstance.Register<ShowErrorMessage>(this, m =>
            {
                Message = m.Message;
            });
        }
    }
}