using System;
using DevTrends.MvcDonutCaching;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Repositories;
using NHibernate;

namespace MegaSite.Api
{
    public sealed class UnitOfWork : IUnitOfWork, IManagers
    {
        private ITransaction _transaction;
        private readonly string _connectionString;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        private ISession _session;

        public ISession Session
        {
            get
            {
                if (_session != null) return _session;

                using (var sessionFactory = NHibernateBuilder.GetSessionFactory(_connectionString))
                {
                    _session = sessionFactory.OpenSession();
                }
                return _session;
            }
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = Session.BeginTransaction();
            }
        }

        //public string Title { get; set; }

        private IRepository<User> _users;
        public IRepository<User> UserRepository
        {
            get { return _users ?? (_users = new Repository<User>(this)); }
        }

        private IRepository<Post> _postRepositoryReader;
        public IRepository<Post> PostRepository
        {
            get { return _postRepositoryReader ?? (_postRepositoryReader = new PostRepository(this)); }
        }

        private IRepository<PostType> _postTypeRepositoryReader;
        public IRepository<PostType> PostTypeRepository
        {
            get
            {
                return _postTypeRepositoryReader ??
                       (_postTypeRepositoryReader = new Repository<PostType>(this));
            }
        }

        private IRepository<Category> _categoryRepositoryReader;
        public IRepository<Category> CategoryRepository
        {
            get
            {
                return _categoryRepositoryReader ??
                       (_categoryRepositoryReader = new Repository<Category>(this));
            }
        }

        private IRepository<Client> _clientRepositoryReader;
        public IRepository<Client> ClientRepository
        {
            get
            {
                return _clientRepositoryReader ?? (_clientRepositoryReader = new Repository<Client>(this));
            }
        }

        private PostManager _postManager;
        public PostManager PostManager
        {
            get { return _postManager ?? (_postManager = new PostManager(this)); }
        }

        private PostTypeManager _postTypeManager;
        public PostTypeManager PostTypeManager
        {
            get { return _postTypeManager ?? (_postTypeManager = new PostTypeManager(this)); }

        }

        private CategoryManager _categoryManager;
        public CategoryManager CategoryManager
        {
            get{ return _categoryManager ?? (_categoryManager = new CategoryManager(this)); }
        }

        private FieldManager _fieldManager;
        public FieldManager FieldManager
        {
            get { return _fieldManager ?? (_fieldManager = new FieldManager()); }
        }

        private UserManager _userManager;
        public UserManager UserManager
        {
            get { return _userManager ?? (_userManager = new UserManager(this)); }
        }

        private ClientManager _clientManager;
        public ClientManager ClientManager
        {
            get { return _clientManager ?? (_clientManager = new ClientManager()); }
        }

        public MediaFileManager MediaFileManager
        {
            get { return MediaFileManager.Instance; }
        }

        public void Commit()
        {
            if (_transaction == null) return;
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            if (_session != null)
            {
                _session.Dispose();
            }
        }
    }
}