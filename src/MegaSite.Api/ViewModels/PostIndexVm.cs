using System.Linq;
using MegaSite.Api.Entities;

namespace MegaSite.Api.ViewModels
{
    public class PostIndexVm
    {
        public PostType PostType { get; set; }
        public object PostFields { get; set; }
        public IQueryable<Post> Posts { get; set; }
    }
}