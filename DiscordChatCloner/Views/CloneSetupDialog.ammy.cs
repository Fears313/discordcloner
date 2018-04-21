using System.Windows;
using DiscordChatCloner.Models;
using DiscordChatCloner.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace DiscordChatCloner.Views
{
    public partial class CloneSetupDialog
    {
        private ICloneSetupViewModel ViewModel => (ICloneSetupViewModel) DataContext;

        public CloneSetupDialog()
        {
            InitializeComponent();
        }

        public void CloneButton_Click(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}