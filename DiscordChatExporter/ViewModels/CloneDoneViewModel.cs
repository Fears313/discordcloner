using System.Diagnostics;
using DiscordChatExporter.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatExporter.ViewModels
{
    public class CloneDoneViewModel : ViewModelBase, ICloneDoneViewModel
    {
        private string _filePath;

        // Commands
        public RelayCommand OpenCommand { get; }

        public CloneDoneViewModel()
        {
            // Commands
            OpenCommand = new RelayCommand(Open);

            // Messages
            MessengerInstance.Register<ShowCloneDoneMessage>(this, m =>
            {
                _filePath = m.FilePath;
            });
        }

        private void Open()
        {
            Process.Start(_filePath);
        }
    }
}