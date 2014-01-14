using System.Collections.Generic;

namespace Meganium.Api.Entities
{
    public class Category : IHaveId, IHaveSlug, IHaveLicense
    {
        public virtual int Id { get; set; }
        public virtual License License { get; set; }

        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }

        public virtual ICollection<Post> Posts {get;set;}

        public virtual PostType PostType { get; set; }
    }
}