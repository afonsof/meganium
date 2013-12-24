using DevTrends.MvcDonutCaching;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Repositories;
using NHibernate;

namespace MegaSite.Api
{
    public sealed class UnitOfWork : IRepositories, IManagers
    {
        private ITransaction _transaction;
        private readonly string _connectionString;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Database

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

        public void Commit()
        {
            if (_transaction == null) return;
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
        }

        #endregion

        #region Repositories

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

        private IRepository<PostType> _postTypeRepository;
        public IRepository<PostType> PostTypeRepository
        {
            get
            {
                return _postTypeRepository ??
                       (_postTypeRepository = new Repository<PostType>(this));
            }
        }

        private IRepository<Category> _categoryRepository;
        public IRepository<Category> CategoryRepository
        {
            get
            {
                return _categoryRepository ??
                       (_categoryRepository = new Repository<Category>(this));
            }
        }

        private IRepository<License> _licenseRepository;
        public IRepository<License> LicenseRepository
        {
            get
            {
                return _licenseRepository ?? (_licenseRepository = new Repository<License>(this));
            }
        }

        private IRepository<Client> _clientRepository;
        public IRepository<Client> ClientRepository
        {
            get
            {
                return _clientRepository ?? (_clientRepository = new Repository<Client>(this));
            }
        }

        private IRepository<ClientSubItem> _clientSubItemRepository;
        public IRepository<ClientSubItem> ClientSubItemRepository
        {
            get
            {
                return _clientSubItemRepository ?? (_clientSubItemRepository = new Repository<ClientSubItem>(this));
            }
        }

        #endregion

        #region Managers

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
            get { return _categoryManager ?? (_categoryManager = new CategoryManager(this)); }
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

        private LicenseManager _licenseManager;
        public LicenseManager LicenseManager
        {
            get { return _licenseManager ?? (_licenseManager = new LicenseManager()); }
        }

        private ClientManager _clientManager;
        public ClientManager ClientManager
        {
            get { return _clientManager ?? (_clientManager = new ClientManager(this)); }
        }

        private ClientSubItemManager _clientSubItemManager;
        public ClientSubItemManager ClientSubItemManager
        {
            get { return _clientSubItemManager ?? (_clientSubItemManager = new ClientSubItemManager(this)); }
        }

        public MediaFileManager MediaFileManager
        {
            get { return MediaFileManager.Instance; }
        }

        #endregion

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