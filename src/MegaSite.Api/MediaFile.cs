using System;
using Dongle.System;

namespace MegaSite.Api
{
    public sealed class MediaFile
    {
        public string Title { get; set; }
        public string Description { get; set; }

        private string _url;
        public string Url
        {
            get
            {
                if (_url == null && FileName != null)
                {
                    _url = "/Content/Uploads/Files/" + FileName + ".jpg";
                }
                return _url;
            }
            set
            {
                _url = value;
            }
        }
        public string ThumbUrl { get; set; }
        public string ExternalServiceId { get; set; }
        public string ExternalServiceName { get; set; }

        private string _fileName;
        public string FileName
        {
            get
            {
                if (String.IsNullOrEmpty(_fileName) && !String.IsNullOrEmpty(Url))
                {
                    return Url.ToMd5();
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