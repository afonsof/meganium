using NHibernate.Validator.Constraints;

namespace Meganium.Api.Entities
{
    public class ClientSubItem : IHaveId, IHaveDataJson
    {
        public virtual int Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Type { get; set; }

        [Length(150000)]
        public virtual string DataJson { get; set; }

        public virtual Client Client { get; set; }
    }
}