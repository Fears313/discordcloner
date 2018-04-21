using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatCloner.ViewModels
{
    public interface ICloneDoneViewModel
    {
        RelayCommand OpenCommand { get; }
    }
}