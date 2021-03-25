using System;
using Xamarin.Forms;

namespace PhotoPOD.Views
{
    public partial class M100_QrScanPage : ContentPage
    {
        public M100_QrScanPage()
        {
            InitializeComponent();
        }

        public void FlashButtonClicked(object sender, EventArgs e)
        {
            scanner.ToggleTorch();
        }
    }
}
