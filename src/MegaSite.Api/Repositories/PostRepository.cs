using System.Linq;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Repositories
{
    class PostRepository: Repository<Post>
    {
        public PostRepository(UnitOfWork uow) : base(uow)
        {
        }

        public override void Add(Post obj)
        {
            if (obj.PostType == null)
            {
                obj.PostType = Uow.PostTypeRepository.AsQueryable().FirstOrDefault(pt => pt.Id == Uow.ClientManager.GetOptions().GetInt("DefaultPostTypeId"));
            }
            base.Add(obj);
        }
    }
}
