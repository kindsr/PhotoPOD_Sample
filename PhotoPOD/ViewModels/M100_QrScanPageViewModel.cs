using PhotoPOD.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace PhotoPOD.ViewModels
{
    public class M100_QrScanPageViewModel : ViewModelBase
    {
        private bool cameraFlashLightOn = false;
        private bool _isAnalyzing = true;
        private bool _isScanning = true;
        private bool _flashButtonVisible;
        private string _topText = "Text";
        private string _bottomText = "Text";

        ZXingScannerView _zxing = new ZXingScannerView();

        public string TopText
        {
            get { return _topText; }
            set { SetProperty(ref _topText, value); }
        }

        public string BottomText
        {
            get { return _bottomText; }
            set { SetProperty(ref _bottomText, value); }
        }

        public bool ShowFlashButton
        {
            get { return _flashButtonVisible; }
            set
            {
                if (!bool.Equals(_flashButtonVisible, value))
                {
                    this._flashButtonVisible = value;
                    SetProperty(ref _flashButtonVisible, value);
                }
            }
        }

        public ZXing.Result Result { get; set; }

        public bool IsAnalyzing
        {
            get { return this._isAnalyzing; }
            set
            {
                if (!bool.Equals(_isAnalyzing, value))
                {
                    _isAnalyzing = value;
                    SetProperty(ref _isAnalyzing, value);
                }
            }
        }

        public bool IsScanning
        {
            get { return _isScanning; }
            set
            {
                if (!bool.Equals(_isScanning, value))
                {
                    this._isScanning = value;
                    SetProperty(ref _isScanning, value);
                }
            }
        }

        public Command QRScanResultCommand => new Command(() =>
        {
            IsAnalyzing = false;
            IsScanning = false;

            Device.BeginInvokeOnMainThread(async () =>
            {
                // Stop analysis until we navigate away so we don't keep reading barcodes
                IsAnalyzing = false;
                IsScanning = false;

                // do something with Result.Text
                if (Result.Text.Count(str => str == '*') == 6)
                {
                    //string[] s = Result.Text.Split('*');
                    GlobalVariables.Instance.ScannedText = Result.Text;
                    var p = new NavigationParameters();
                    p.Add("scannedText", Result.Text);
                    await NavigationService.NavigateAsync("M200_SelectLayoutPage", p);
                }
                else
                    await NavigationService.GoBackAsync();
            });
        });

        public M100_QrScanPageViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            Title = "QR Scan";
            ShowFlashButton = true;
            TopText = "Scan the QR code on the PhotoPOD machine.";

            CanNavigate = true;
            IsAnalyzing = true;
            IsScanning = true;
        }

        protected override void NavigateCommandExecute()
        {
            var p = new NavigationParameters();
            if (Result == null)
            {
                GlobalVariables.Instance.ScannedText = "*iRoboTech*iBeautyNail*PhotoPOD*1*7f847cf2-8a25-4e33-9f08-581f4c2d9118*";
                p.Add("scannedText", "*iRoboTech*iBeautyNail*PhotoPOD*1*7f847cf2-8a25-4e33-9f08-581f4c2d9118*");
            }
            else
            {
                if (Result.Text.Count(str => str == '*') == 6)
                {
                    p.Add("scannedText", Result.Text);
                }
            }

            NavigationService.NavigateAsync("M200_SelectLayoutPage", p);
        }
    }
}
