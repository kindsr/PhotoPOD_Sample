using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PhotoPOD.Models
{
    public partial class Photo
    {
        [JsonProperty("p_uid")]
        public string UID { get; set; }

        [JsonProperty("p_machine_id")]
        public int MachineId { get; set; }

        [JsonProperty("p_layout")]
        public int LayoutSeq { get; set; }

        [JsonProperty("p_file_path")]
        public string FilePath { get; set; }
        
        [JsonProperty("p_img_bytes")]
        public byte[] ImageBytes { get; set; }

        [JsonProperty("p_img_base64")]
        public string ImageBase64 { get; set; }

        [JsonProperty("p_img_src")]
        public ImageSource ImageSrc { get; set; }

        [JsonProperty("p_reg_dt")]
        public DateTimeOffset? RegDt { get; set; }
    }

    public partial class Photo
    {
        public static Photo[] FromJson(string json) => JsonConvert.DeserializeObject<Photo[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Photo[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
