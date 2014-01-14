using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MegaSite.Api.Entities;
using MegaSite.Api.Managers;
using MegaSite.Api.Repositories;
using NHibernate.Linq;

namespace MegaSite.Api
{
    public sealed class UnitOfWork : IRepositories, IManagers
    {
        private static readonly Dictionary<int, License> Licenses = new Dictionary<int, License>(); 

        private License _license;

        public License License
        {
            get
            {
                if (_license != null)
                {
                    return _license;
                }

                int? licenseId = null;
                var httpContext = HttpContext.Current;

                if (httpContext.Session != null && httpContext.Session["licenseId"] != null)
                {
                    licenseId = Convert.ToInt32(httpContext.Session["licenseId"]);
                }

                if (!licenseId.HasValue)
                {
                    if (httpContext.User.Identity.IsAuthenticated)
                    {
                        var user = Db.Session.Query<User>().FirstOrDefault(u=>u.UserName == httpContext.User.Identity.Name);
                        if (user != null && user.License != null)
                        {
                            licenseId = user.License.Id;
                        }
                    }
                }
                if (!licenseId.HasValue)
                {
                    _license = LicenseManager.GetByUrl(httpContext.Request.Url.Authority);
                    if (_license != null)
                    {
                        Licenses[_license.Id] = _license;
                        if (httpContext.Session != null)
                        {
                            httpContext.Session["licenseId"] = _license.Id;
                        }
                        return _license;
                    }
                }
                if (!licenseId.HasValue)
                {
                    var value = ConfigurationManager.AppSettings.Get("DefaultLicenseId");
                    if (!string.IsNullOrEmpty(value))
                    {
                        licenseId = Convert.ToInt32(value);
                    }
                }

                if (licenseId.HasValue)
                {
                    if (httpContext.Session != null)
                    {
                        httpContext.Session["licenseId"] = licenseId.Value;
                    }
                    if (Licenses.ContainsKey(licenseId.Value))
                    {
                        _license = Licenses[licenseId.Value];
                        return _license;
                    }
                    _license = LicenseRepository.GetById(licenseId.Value);
                    if (_license != null)
                    {
                        Licenses[licenseId.Value] = _license;
                        return _license;
                    }
                }
                return new License();
            }
        }
        public Database Db { get; private set; }

        public UnitOfWork()
        {
            Db = new Database(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        #region Repositories

        private IRepository<User> _users;
        public IRepository<User> UserRepository
        {
            get { return _users ?? (_users = new LicensedRepository<User>(Db, this)); }
        }

        private IRepository<Post> _postRepositoryReader;
        public IRepository<Post> PostRepository
        {
            get { return _postRepositoryReader ?? (_postRepositoryReader = new LicensedRepository<Post>(Db, this)); }
        }

        private IRepository<PostType> _postTypeRepository;
        public IRepository<PostType> PostTypeRepository
        {
            get
            {
                return _postTypeRepository ??
                       (_postTypeRepository = new LicensedRepository<PostType>(Db, this));
            }
        }

        private IRepository<Category> _categoryRepository;
        public IRepository<Category> CategoryRepository
        {
            get
            {
                return _categoryRepository ??
                       (_categoryRepository = new LicensedRepository<Category>(Db, this));
            }
        }

        private IRepository<License> _licenseRepository;
        public IRepository<License> LicenseRepository
        {
            get
            {
                return _licenseRepository ?? (_licenseRepository = new Repository<License>(Db));
            }
        }

        private IRepository<Client> _clientRepository;
        public IRepository<Client> ClientRepository
        {
            get
            {
                return _clientRepository ?? (_clientRepository = new LicensedRepository<Client>(Db, this));
            }
        }

        private IRepository<ClientSubItem> _clientSubItemRepository;
        public IRepository<ClientSubItem> ClientSubItemRepository
        {
            get
            {
                return _clientSubItemRepository ?? (_clientSubItemRepository = new Repository<ClientSubItem>(Db));
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
            get { return _postTypeManager ?? (_postTypeManager = new PostTypeManager(this, License)); }

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
            get { return _licenseManager ?? (_licenseManager = new LicenseManager(this)); }
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

        public void Commit()
        {
            Db.Commit();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}