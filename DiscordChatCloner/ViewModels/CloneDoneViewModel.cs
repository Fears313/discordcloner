using System.Diagnostics;
using DiscordChatCloner.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public class CloneDoneViewModel : ViewModelBase, ICloneDoneViewModel
    {

        // Commands
        public RelayCommand OpenCommand { get; }

        public CloneDoneViewModel()
        {
            // Messages
            MessengerInstance.Register<ShowCloneDoneMessage>(this, m =>
            {
            });
        }
    }
}