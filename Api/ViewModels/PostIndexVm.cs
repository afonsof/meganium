using System.Linq;
using Meganium.Api.Entities;

namespace Meganium.Api.ViewModels
{
    public class PostIndexVm
    {
        public PostType PostType { get; set; }
        public object PostFields { get; set; }
        public IQueryable<Post> Posts { get; set; }
    }
}