using PhotoPOD.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace PhotoPOD.ViewModels
{
    public class M000_StartPageViewModel : ViewModelBase
    {
        public M000_StartPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            Title = "Photo POD";
            CanNavigate = true;
        }

        protected override void NavigateCommandExecute()
        {
            NavigationService.NavigateAsync("M100_QrScanPage");
        }
    }
}
