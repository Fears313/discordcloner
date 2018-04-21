using System.Windows;
using MaterialDesignThemes.Wpf;

namespace DiscordChatCloner.Views
{
    public partial class CloneDoneDialog
    {
        public CloneDoneDialog()
        {
            InitializeComponent();
        }

        public void OpenButton_Click(object sender, RoutedEventArgs args)
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
