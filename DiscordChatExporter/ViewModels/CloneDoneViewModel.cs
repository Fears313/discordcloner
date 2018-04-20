using System.Diagnostics;
using DiscordChatExporter.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatExporter.ViewModels
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