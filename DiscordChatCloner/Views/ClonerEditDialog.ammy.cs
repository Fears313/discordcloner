using System.Windows;
using DiscordChatCloner.Models;
using DiscordChatCloner.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace DiscordChatCloner.Views
{
    public partial class ClonerEditDialog
    {
        private IClonerEditViewModel ViewModel => (IClonerEditViewModel) DataContext;

        public ClonerEditDialog()
        {
            InitializeComponent();
        }

        public void DeleteButton_Click(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}