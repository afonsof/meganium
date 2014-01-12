using System.Linq;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;

namespace MegaSite.Api.Managers
{
    public class LicenseManager
    {
        private readonly IRepositories _repositories;

        public LicenseManager(IRepositories repositories)
        {
            _repositories = repositories;
        }

        public License GetById(int id)
        {
            return _repositories.LicenseRepository.GetById(id);
        }

        public License GetByUrl(string domain)
        {
            return _repositories.LicenseRepository.AsQueryable().FirstOrDefault(l=>l.Domain == domain);
        }

        public License GetByName(string name)
        {
            return _repositories.LicenseRepository.AsQueryable().FirstOrDefault(l => l.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public License Create(string theme)
        {
            var license = new License
            {
                Name = theme
            };
            _repositories.LicenseRepository.Add(license);
            _repositories.Commit();
            return license;
        }
    }
}