using FFImageLoading.Forms;
using Newtonsoft.Json;
using PhotoPOD.Models;
using Plugin.Media;
//using PhotoPOD.Services;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PhotoPOD.ViewModels
{
    public class M300_UploadImagesPageViewModel : ViewModelBase//, INotifyPropertyChanged
    {
        private const string Host = "http://192.168.0.105:8080/";
        private const string CreateTempPicture = "nailpod/createTempPicture";

        HttpClient httpClient;
        HttpClient Client => httpClient ?? (httpClient = new HttpClient());

        //IMultiMediaPickerService _multiMediaPickerService;
        public ObservableCollection<MediaFile> Media { get; set; }
        public ObservableCollection<Photo> MediaList { get; set; }
        //public ICommand SelectImagesCommand { get; set; }
        //public ICommand SelectVideosCommand { get; set; }

        public bool CanExecuteCommand() { return true; }

        private int _layoutNumber;
        public int LayoutNumber
        {
            get { return _layoutNumber; }
            set { SetProperty(ref _layoutNumber, value); }
        }

        private string _scannedText;
        public string ScannedText
        {
            get { return _scannedText; }
            set { SetProperty(ref _scannedText, value); }
        }

        public M300_UploadImagesPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Upload Image";
            Media = new ObservableCollection<MediaFile>();
            MediaList = new ObservableCollection<Photo>();
            Media.CollectionChanged += Files_CollectionChanged;
        }

        private DelegateCommand _selectImagesCommand;
        public DelegateCommand SelectImagesCommand =>
            _selectImagesCommand ?? (_selectImagesCommand = new DelegateCommand(SelectImagesCommandExecute, CanExecuteCommand));

        public async void SelectImagesCommandExecute()
        {
            await CrossMedia.Current.Initialize();
            Media.Clear();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var picked = await CrossMedia.Current.PickPhotosAsync();

            if (picked == null)
                return;
            foreach (var file in picked)
            {
                //Media.Add(file);
                Device.BeginInvokeOnMainThread(() =>
                {
                    Media.Add(file);
                });
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ScannedText = (string)parameters["scannedText"] ?? GlobalVariables.Instance.ScannedText;
            LayoutNumber = Int32.Parse(parameters["selectedLayout"].ToString().Split(' ')[1]);

            Title = _layoutNumber > 1 ? Title + "s" : Title;
        }

        protected override void ClickAction(string action)
        {
            Task.Run(async () => await PostNailpodsAsync());
        }

        private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Media.Count == 0)
            {
                MediaList.Clear();
                return;
            }
            if (e.NewItems.Count == 0)
                return;

            var file = e.NewItems[0] as MediaFile;
            var image = new CachedImage { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
            //image.Source = ImageSource.FromFile(file.Path);
            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            var imgBytes = ImageSourceToByteArray(image.Source);
            MediaList.Add(new Photo() { FilePath = file.Path, ImageSrc = image.Source, ImageBytes = imgBytes, ImageBase64 = Convert.ToBase64String(imgBytes) });

            //var image2 = new CachedImage { WidthRequest = 300, HeightRequest = 300, Aspect = Aspect.AspectFit };
            //image2.Source = ImageSource.FromFile(file.Path);
            //ImageList.Children.Add(image2);
        }

        private byte[] ImageSourceToByteArray(ImageSource source)
        {
            StreamImageSource streamImageSource = (StreamImageSource)source;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;

            byte[] b;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                b = ms.ToArray();
            }

            return b;
        }


        async Task PostNailpodsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                int i = 0;

                Photo[] photoArray = new Photo[MediaList.Count];

                foreach(var m in MediaList)
                {
                    var req = new Photo
                    {
                        UID = ScannedText.Split('*')[5],
                        MachineId = Int32.Parse(ScannedText.Split('*')[4]),
                        LayoutSeq = i + 1,
                        FilePath = m.FilePath,
                        //ImageBytes = m.ImageBytes,
                        ImageBase64 = m.ImageBase64,
                        RegDt = DateTime.Now
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                    var json = await Client.PostAsync(Host + CreateTempPicture, content);

                    if (!json.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error!", json.ReasonPhrase, "OK");
                    }

                    //photoArray[i] = req;
                    i++;
                }

                //HttpContent content = new StringContent(Serialize.ToJson(photoArray), Encoding.UTF8, "application/json");
                //var json = await Client.PostAsync(Host + CreateTempPicture, content);
                await NavigationService.NavigateAsync("M000_StartPage");
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Unable to save: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
