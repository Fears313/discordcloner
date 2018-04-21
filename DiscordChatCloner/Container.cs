using DiscordChatCloner.Services;
using DiscordChatCloner.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DiscordChatCloner
{
    public class Container
    {
        public static void Init()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Settings
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();
            ServiceLocator.Current.GetInstance<ISettingsService>().Load();

            // Services
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<ICloneService, CloneService>();
            SimpleIoc.Default.Register<IMessageGroupService, MessageGroupService>();

            // View models
            SimpleIoc.Default.Register<IErrorViewModel, ErrorViewModel>(true);
            SimpleIoc.Default.Register<ICloneDoneViewModel, CloneDoneViewModel>(true);
            SimpleIoc.Default.Register<ICloneSetupViewModel, CloneSetupViewModel>(true);
            SimpleIoc.Default.Register<IMainViewModel, MainViewModel>(true);
            SimpleIoc.Default.Register<ISettingsViewModel, SettingsViewModel>(true);
        }

        public static void Cleanup()
        {
            // Settings
            ServiceLocator.Current.GetInstance<ISettingsService>().Save();
        }

        public IErrorViewModel ErrorViewModel => ServiceLocator.Current.GetInstance<IErrorViewModel>();
        public ICloneDoneViewModel CloneDoneViewModel => ServiceLocator.Current.GetInstance<ICloneDoneViewModel>();
        public ICloneSetupViewModel CloneSetupViewModel => ServiceLocator.Current.GetInstance<ICloneSetupViewModel>();
        public IMainViewModel MainViewModel => ServiceLocator.Current.GetInstance<IMainViewModel>();
        public ISettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<ISettingsViewModel>();
    }
}