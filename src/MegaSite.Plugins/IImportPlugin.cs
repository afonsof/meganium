using System.Collections.Generic;
using System.Collections.Specialized;
using MegaSite.Api;

namespace MegaSite.Plugins
{
    public interface IImportPlugin
    {
        IEnumerable<ImportPost> ReadPosts(NameValueCollection values);
        IEnumerable<MediaFile> ReadMediaFiles(ImportPost post, NameValueCollection values);
    }
}