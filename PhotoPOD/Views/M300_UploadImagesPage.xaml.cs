//using PhotoPOD.Services;
using System;
using System.IO;
using Xamarin.Forms;

namespace PhotoPOD.Views
{
    public partial class M300_UploadImagesPage : ContentPage
    {
        public M300_UploadImagesPage()
        {
            InitializeComponent();
        }

        //async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        //{
        //    (sender as Button).IsEnabled = false;

        //    Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
        //    if (stream != null)
        //    {
        //        image.Source = ImageSource.FromStream(() => stream);
        //    }

        //    (sender as Button).IsEnabled = true;
        //}
    }
}
