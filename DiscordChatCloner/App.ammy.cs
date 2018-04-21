using System.Windows;

namespace DiscordChatCloner
{
    public partial class App
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            Container.Init();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Container.Cleanup();
        }
    }
}