using System.IO;
using System.Threading.Tasks;

namespace PhotoPOD.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
