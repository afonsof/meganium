using System;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Repositories
{
    public interface IRepositories : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Post> PostRepository { get; }
        IRepository<PostType> PostTypeRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<License> LicenseRepository { get; }
        IRepository<Client> ClientRepository { get; }
        IRepository<ClientSubItem> ClientSubItemRepository { get; }
        Database Db { get; }
        void Commit();
    }
}