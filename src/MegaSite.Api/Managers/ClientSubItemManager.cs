using System.Linq;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;

namespace MegaSite.Api.Managers
{
    public class ClientSubItemManager
    {
        private readonly IRepositories _repositories;

        public ClientSubItemManager(IRepositories repositories)
        {
            _repositories = repositories;
        }

        public ClientSubItem GetByCode(string code)
        {
            return _repositories
                .ClientSubItemRepository
                .AsQueryable()
                .FirstOrDefault(c => c.Code == code);
        }

        public void Change(ClientSubItem guest)
        {
            _repositories.ClientSubItemRepository.Edit(guest);
            _repositories.Commit();
        }
    }
}