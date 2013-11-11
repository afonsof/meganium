using System.Linq;
using MegaSite.Api.Entities;

namespace MegaSite.Api.ViewModels
{
    public class SiteViewModel
    {
        public SiteViewModel(IManagers managers)
        {
            Managers = managers;
        }

        public IManagers Managers { get; internal set; }
        public string Title { get; set; }
        public Post CurrentPost { get; set; }
        public IQueryable<Post> CurrentPosts { get; set; }
        public Category CurrentCategory { get; set; }
    }
}