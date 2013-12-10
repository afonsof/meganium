using System;
using MegaSite.Api.Managers;

namespace MegaSite.Api
{
    public interface IManagers: IDisposable
    {
        PostManager PostManager { get; }
        PostTypeManager PostTypeManager { get; }
        CategoryManager CategoryManager { get; }
        FieldManager FieldManager { get; }
        UserManager UserManager { get; }
        MediaFileManager MediaFileManager { get; }
        ClientManager ClientManager { get; }
    }

    public class ClientManager
    {
        public IOptions GetOptions()
        {
            return Options.Instance;
        }
    }
}