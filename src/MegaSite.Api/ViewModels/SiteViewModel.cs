using System.Linq;
using MegaSite.Api.Entities;
using MegaSite.Api.Plugins;

namespace MegaSite.Api.ViewModels
{
    public class SiteViewModel
    {
        public SiteViewModel(IManagers managers, IActionPluginManager pluginManager)
        {
            PluginManager = pluginManager;
            Managers = managers;
        }

        public IActionPluginManager PluginManager { get; private set; }
        public IManagers Managers { get; private set; }
        public string Title { get; set; }
        public Post CurrentPost { get; set; }
        public IQueryable<Post> CurrentPosts { get; set; }
        public Category CurrentCategory { get; set; }
    }
}