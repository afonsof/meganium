using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Dongle.System;
using Dongle.System.IO;
using Encoder = System.Drawing.Imaging.Encoder;

namespace MegaSite.Api.Managers
{
    public class MediaFileManager
    {
        private readonly DirectoryInfo _rawFilesPath;
        private readonly string _rootPath;
        private readonly string _rawFilesRelPath;
        private readonly string _thumbFilesRelPath;
        private readonly string _rootUrl;

        private MediaFileManager(string rootPath, string rawFilesRelPath, string thumbFilesRelPath, string rootUrl)
        {
            _rawFilesPath = new DirectoryInfo(Path.Combine(rootPath, rawFilesRelPath));
            _rawFilesPath.CreateRecursively();

            _rootPath = rootPath;
            _rawFilesRelPath = rawFilesRelPath;
            _thumbFilesRelPath = thumbFilesRelPath;
            _rootUrl = rootUrl;
        }

        public static void Setup(string rootPath, string rawFilesRelPath, string thumbFilesRelPath, string rootUrl)
        {
            _instance = new MediaFileManager(rootPath, rawFilesRelPath, thumbFilesRelPath, rootUrl);
        }

        private static MediaFileManager _instance;
        public static MediaFileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("Setup was not called in MediaFileManager");
                }
                return _instance;
            }
        }

        public static bool SetupWasCalled()
        {
            return _instance != null;
        }

        public MediaFile Save(HttpPostedFileBase postedFile)
        {
            var fileName = (postedFile.FileName + postedFile.ContentLength).ToMd5();
            var fileInfo = GetPath(fileName);
            _rawFilesPath.CreateRecursively();

            using (var image = Image.FromStream(postedFile.InputStream))
            {
                CreateThumb(image, fileInfo.FullName, 1024, 1024, false);
            }
            return new MediaFile
            {
                Title = postedFile.FileName,
                FileName = fileName
            };
        }

        public string GetThumbPath(string fileName, int width, int height, bool crop)
        {
            var sb =
                new StringBuilder(_thumbFilesRelPath).Append('\\')
                    .Append(fileName)
                    .Append('-')
                    .Append(width)
                    .Append('x')
                    .Append(height);
            if (crop) sb.Append("-crop");
            sb.Append(".jpg");
            return sb.ToString();
        }

        public string GetThumb(string url, string fileName, int width, int height, bool crop)
        {
            var thumbFilePath = Path.Combine(_rootPath, GetThumbPath(fileName, width, height, crop));

            if (File.Exists(thumbFilePath))
            {
                return thumbFilePath;
            }
            var filePath = Path.Combine(_rawFilesPath.FullName, fileName + ".jpg");

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                if (url != null)
                {
                    url = url.FromBase64ToString();
                    if (!url.ToLowerInvariant().StartsWith("http://") && !url.ToLowerInvariant().StartsWith("https://"))
                    {
                        url = _rootUrl + (url.StartsWith("/") ? url : ("/" + url));
                    }
                    _rawFilesPath.CreateRecursively();
                    var webClient = new WebClient();
                    webClient.DownloadFile(url, filePath);
                }
            }
            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                CreateThumb(filePath, thumbFilePath, width, height, crop);
                if (File.Exists(thumbFilePath))
                {
                    return thumbFilePath;
                }
            }
            return null;
        }


        public static Tuple<Size, Size> GetResizeData(int imageWidth, int imageHeight, int width, int height, bool crop)
        {
            width = width > 3000 ? 3000 : width;
            height = height > 3000 ? 3000 : height;

            if (crop)
            {
                width = width > imageWidth ? imageWidth : width;
                height = height > imageHeight ? imageHeight : height;
            }

            var x = width / (float)imageWidth;
            var y = height / (float)imageHeight;
            int destWidth;
            int destHeight;

            if (crop)
            {
                if (x > y || height == 0)
                {
                    destWidth = width;
                    destHeight = (int)(x * imageHeight);
                    if (height == 0)
                    {
                        height = destHeight;
                    }
                }
                else
                {
                    destWidth = (int)(y * imageWidth);
                    destHeight = height;
                    if (width == 0)
                    {
                        width = destWidth;
                    }
                }
            }
            else if ((x < y && width > 0) || height == 0)
            {
                destWidth = width;
                destHeight = (int)(x * imageHeight);
                height = destHeight;
            }
            else
            {
                destWidth = (int)(y * imageWidth);
                destHeight = height;
                width = destWidth;
            }
            return new Tuple<Size, Size>(new Size(destWidth, destHeight), new Size(width, height));
        }

        public List<MediaFile> GetRecents()
        {
            return _rawFilesPath
                .GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .Select(f => new MediaFile
                {
                    FileName = f.Name.Replace(".jpg", "")
                }).ToList();
        }

        #region PrivateMethods
        private static void CreateThumb(string fromFile, string toFile, int width, int height, bool crop)
        {
            var fileInfo = new FileInfo(fromFile);
            using (var image = Image.FromFile(fileInfo.FullName))
            {
                CreateThumb(image, toFile, width, height, crop);
            }
        }

        private static void CreateThumb(Image image, string toFile, int width, int height, bool crop)
        {
            new FileInfo(toFile).Directory.CreateRecursively();
            using (var thumbnailImage = ResizeImage(image, width, height, crop))
            {
                SaveJpeg(toFile, thumbnailImage, 95);
            }
        }

        private static Image ResizeImage(Image image, int width, int height, bool crop)
        {
            var resizeData = GetResizeData(image.Width, image.Height, width, height, crop);
            var bitmap = ResizeImage(image, resizeData.Item1.Width, resizeData.Item1.Height);
            if (resizeData.Item1.Width != resizeData.Item2.Width || resizeData.Item1.Height != resizeData.Item2.Height)
            {
                var img = CropImage(bitmap, resizeData.Item2.Width, resizeData.Item2.Height);
                bitmap.Dispose();
                return img;
            }
            return bitmap;
        }

        private static Dictionary<string, ImageCodecInfo> _encoders;
        private static Dictionary<string, ImageCodecInfo> Encoders
        {
            get
            {
                if (_encoders == null)
                {
                    _encoders = new Dictionary<string, ImageCodecInfo>();
                }

                if (_encoders.Count == 0)
                {
                    foreach (var codec in ImageCodecInfo.GetImageEncoders())
                    {
                        _encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }
                return _encoders;
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var result = new Bitmap(width, height);
            result.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }
            return result;
        }

        private static Image CropImage(Image image, int width, int height)
        {
            if (image.Width >= width && image.Height >= height)
            {
                var x = (image.Width / 2) - (width / 2);
                var y = (image.Height / 2) - (height / 2);

                using (var bitmap = new Bitmap(image))
                {
                    var croppedImage = bitmap.Clone(new Rectangle(x, y, width, height), PixelFormat.Format32bppArgb);
                    return croppedImage;
                }
            }
            else
            {
                var x = (width / 2) - (image.Width / 2);
                var y = (height / 2) - (image.Height / 2);

                var bitmap = new Bitmap(width, height);
                var g = Graphics.FromImage(bitmap);
                g.DrawImage(image, x, y);
                return bitmap;
            }
        }

        private static void SaveJpeg(string path, Image image, int quality)
        {
            if ((quality < 0) || (quality > 100))
            {
                var error = string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality);
                throw new ArgumentOutOfRangeException(error);
            }

            var qualityParam = new EncoderParameter(Encoder.Quality, quality);
            var jpegCodec = GetEncoderInfo("image/jpeg");

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            image.Save(path, jpegCodec, encoderParams);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var lookupKey = mimeType.ToLower();
            ImageCodecInfo foundCodec = null;

            if (Encoders.ContainsKey(lookupKey))
            {
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }

        private FileInfo GetPath(string fileName)
        {
            var path = Path.Combine(_rawFilesPath.FullName, fileName + ".jpg");
            return new FileInfo(path);
        }
        #endregion
    }
}