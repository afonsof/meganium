using System;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;

namespace MegaSite.Api
{
    public interface IRepositories : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Post> PostRepository { get; }
        IRepository<PostType> PostTypeRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<License> LicenseRepository { get; }
        IRepository<Client> ClientRepository { get; }
        void Commit();
    }
}