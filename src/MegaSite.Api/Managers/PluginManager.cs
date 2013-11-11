using System.Collections.Generic;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Managers
{
    public class PluginManager
    {
        private readonly IUnitOfWork _uow;

        public PluginManager(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void CreateAndSave(IEnumerable<Plugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                _uow.PluginRepository.Add(plugin);
            }
            _uow.Commit();
        }
    }
}