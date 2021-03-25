using DLToolkit.Forms.Controls;
//using PhotoPOD.Services;
using PhotoPOD.ViewModels;
using PhotoPOD.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace PhotoPOD
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            FlowListView.Init();
            await NavigationService.NavigateAsync("NavigationPage/M000_StartPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<M000_StartPage, M000_StartPageViewModel>();
            containerRegistry.RegisterForNavigation<M100_QrScanPage, M100_QrScanPageViewModel>();
            containerRegistry.RegisterForNavigation<M200_SelectLayoutPage, M200_SelectLayoutPageViewModel>();
            containerRegistry.RegisterForNavigation<M300_UploadImagesPage, M300_UploadImagesPageViewModel>();
        }
    }
}
