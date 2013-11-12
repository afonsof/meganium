using System;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;

namespace MegaSite.Api
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Post> PostRepository { get; }
        IRepository<PostType> PostTypeRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Client> ClientRepository { get; }
        IRepository<Plugin> PluginRepository { get; }

        void Commit();
    }
}