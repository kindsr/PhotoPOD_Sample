using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoPOD.Models;

namespace PhotoPOD.Services
{
    public interface IMultiMediaPickerService
    {
        event EventHandler<MediaFile> OnMediaPicked;
        event EventHandler<IList<MediaFile>> OnMediaPickedCompleted;
        Task<IList<MediaFile>> PickPhotosAsync();
        Task<IList<MediaFile>> PickVideosAsync();
        void Clean();
    }


}
