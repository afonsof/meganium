namespace MegaSite.Api.Entities
{
    public class ClientSubItem : IHaveId
    {
        public virtual int Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Type { get; set; }

        public virtual string DataJson { get; set; }
    }
}