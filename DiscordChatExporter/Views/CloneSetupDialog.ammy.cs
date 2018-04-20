using System.Windows;
using DiscordChatExporter.Models;
using DiscordChatExporter.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace DiscordChatExporter.Views
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