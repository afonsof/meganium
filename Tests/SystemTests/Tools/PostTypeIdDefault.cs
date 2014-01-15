using Meganium.Api;
using Meganium.Api.Entities;

namespace Meganium.SystemTests.Tools
{
    public static class PostTypeIdDefault
    {
        private static int _type;

        public static int Get()
        {
            if (_type != 0)
            {
                return _type;
            }
            using (var uow = new UnitOfWork())
            {
                var postType = uow.PostTypeManager.GetBySingularName("Receita");
                if (postType == null)
                {
                    postType = new PostType
                    {
                        SingularName = "Receita",
                        PluralName = "Receitas",
                        IconId = "book"
                    };
                    postType.SetBehavior(
                        PostType.BehaviorFlags.AllowCategories |
                        PostType.BehaviorFlags.AllowComments |
                        PostType.BehaviorFlags.AllowDescription |
                        PostType.BehaviorFlags.AllowExternalId |
                        PostType.BehaviorFlags.AllowHash |
                        PostType.BehaviorFlags.AllowLocation |
                        PostType.BehaviorFlags.AllowMarkAsFeatured |
                        PostType.BehaviorFlags.AllowPhotos |
                        PostType.BehaviorFlags.AllowTimeBox |
                        PostType.BehaviorFlags.AllowTree |
                        PostType.BehaviorFlags.AllowVideo);

                    uow.PostTypeRepository.Add(postType);
                    uow.Commit();

                    TestToolkit.Managers.License.Options.Set("DefaultPostTypeId", postType.Id);
                    return _type = postType.Id;
                }
            }
            return 0;
        }
    }
}