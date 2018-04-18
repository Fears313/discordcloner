using GalaSoft.MvvmLight.CommandWpf;

namespace DiscordChatExporter.ViewModels
{
    public interface ICloneDoneViewModel
    {
        RelayCommand OpenCommand { get; }
    }
}