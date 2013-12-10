using System.Collections.Generic;
using System.Collections.Specialized;

namespace MegaSite.Api.Plugins
{
    public interface IImportPlugin
    {
        IEnumerable<ImportPost> ReadPosts(NameValueCollection values);
        IEnumerable<MediaFile> ReadMediaFiles(ImportPost post, NameValueCollection values);
        ImportPluginType Type { get; }
    }
}