using PhotoPOD.Models;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace PhotoPOD.ViewModels
{
    public class M200_SelectLayoutPageViewModel : ViewModelBase
    {
        private string _scannedText;
        public string ScannedText
        {
            get { return _scannedText; }
            set { SetProperty(ref _scannedText, value); }
        }

        public M200_SelectLayoutPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Select Layout";
        }

        protected override void ClickAction(string action)
        {
            var p = new NavigationParameters();
            p.Add("scannedText", ScannedText);
            p.Add("selectedLayout", action);
            NavigationService.NavigateAsync("M300_UploadImagesPage", p);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ScannedText = (string)parameters["scannedText"] ?? GlobalVariables.Instance.ScannedText;
        }
    }
}
