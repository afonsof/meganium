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

        //todo:
        /* IQueryable<Post> CurrentPosts { get; set; }
            Post CurrentPost { get; set; }
            Category CurrentCategory { get; set; }
            ActionPluginComposite Plugin { get; set; }
            string Title { get; set; }

            void SaveCategoriesToPost(Post post, List<int> categoriesIds);*/
    }
}