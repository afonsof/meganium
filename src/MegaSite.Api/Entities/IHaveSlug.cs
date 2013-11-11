namespace MegaSite.Api.Entities
{
    public interface IHaveSlug: IHaveTitle
    {
        string Slug { get; set; }
    }
}