using System;
using Dongle.System;
using Newtonsoft.Json;

namespace Meganium.Api
{
    public sealed class MediaFile
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string ExtUrl { get; set; }

        [JsonIgnore]
        public string Url
        {
            get
            {
                if (ExtUrl == null && FileName != null)
                {
                    return "/Content/Uploads/Files/" + FileName + ".jpg";
                }
                return ExtUrl;
            }
        }
        public string ThumbUrl { get; set; }
        public string ExtId { get; set; }
        public string ExtName { get; set; }

        private string _fileName;
        public string FileName
        {
            get
            {
                if (String.IsNullOrEmpty(_fileName) && !String.IsNullOrEmpty(ExtUrl))
                {
                    return ExtUrl.ToMd5();
                }
                return _fileName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _fileName = value;
                }
            }
        }

        private bool Equals(MediaFile other)
        {
            if (other == null)
            {
                return false;
            }
            return string.Equals(FileName, other.FileName);
        }

        public override bool Equals(object obj)
        {
            var a = this;
            var b = obj as MediaFile;

            if (b == null)
            {
                return false;
            }

            if (ReferenceEquals(a, b))
            {
                return true;
            }
            return a.FileName == b.FileName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return FileName != null ? FileName.GetHashCode() : 0;
            }
        }

        public static bool operator ==(MediaFile a, MediaFile b)
        {
            if ((object)a == null)
            {
                if ((object)b == null)
                {
                    return true;
                }
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(MediaFile a, MediaFile b)
        {
            return !(a == b);
        }
    }
}