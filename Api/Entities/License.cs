using NHibernate.Validator.Constraints;

namespace Meganium.Api.Entities
{
    public class License: IHaveId
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Domain { get; set; }
        [Length(10000)]
        public virtual string OptionsJson { get; set; }

        public virtual IOptions Options
        {
            get { return Api.Options.GetOptions(Id); }
        }
    }
}