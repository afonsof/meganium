using NHibernate.Validator.Constraints;

namespace MegaSite.Api.Entities
{
    public class Plugin: IHaveId, IHaveTitle
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual PostType DefaultPostType { get; set; }

        [Length(1024)]
        public virtual string FieldsJson { get; set; }
    }
}
