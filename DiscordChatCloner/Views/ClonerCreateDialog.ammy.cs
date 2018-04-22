using System.Windows;
using DiscordChatCloner.ViewModels;
using MaterialDesignThemes.Wpf;

namespace DiscordChatCloner.Views
{
    public partial class ClonerCreateDialog
    {
        private IClonerCreateViewModel ViewModel => (IClonerCreateViewModel) DataContext;

        public ClonerCreateDialog()
        {
            InitializeComponent();
        }

        public void CreateButton_Click(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}