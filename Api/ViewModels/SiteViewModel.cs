using System.Collections.Generic;
using System.Linq;
using Meganium.Api.Entities;
using Meganium.Api.Managers;
using Meganium.Api.Plugins;

namespace Meganium.Api.ViewModels
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

        private IQueryable<Post> _currentPosts = new List<Post>().AsQueryable();
        public IQueryable<Post> CurrentPosts {
            get
            {
                return _currentPosts;
            }
            set
            {
                _currentPosts = value;
            }
        }
        public Category CurrentCategory { get; set; }
    }
}