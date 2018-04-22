using System.Reflection;
using DiscordChatCloner.Messages;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Tyrrrz.Extensions;

namespace DiscordChatCloner.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Title += $" v{Assembly.GetExecutingAssembly().GetName().Version}";

            // Dialogs
            Messenger.Default.Register<ShowErrorMessage>(this,
                m => DialogHost.Show(new ErrorDialog()).Forget());
            Messenger.Default.Register<ShowCloneDoneMessage>(this,
                m => DialogHost.Show(new CloneDoneDialog()).Forget());
            Messenger.Default.Register<ShowCloneSetupMessage>(this,
                m => DialogHost.Show(new CloneSetupDialog()).Forget());
            Messenger.Default.Register<ShowSettingsMessage>(this,
                m => DialogHost.Show(new SettingsDialog()).Forget());
        }
    }
}