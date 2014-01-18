using System;
using Meganium.Api.Entities;

namespace Meganium.Api.Managers
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
        ClientSubItemManager ClientSubItemManager { get; }
        LicenseManager LicenseManager { get; }

        License License { get; }
        void ClearCache();
    }
}