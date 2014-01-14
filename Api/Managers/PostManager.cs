using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Dongle.System;
using Meganium.Api.Entities;
using Meganium.Api.Repositories;
using Meganium.Api.Trash;
using MySql.Data.MySqlClient;

namespace Meganium.Api.Managers
{
    public class PostManager
    {
        private readonly IRepositories _uow;

        public PostManager(IRepositories uow)
        {
            _uow = uow;
        }

        public Post Create(PostType postType)
        {
            var now = DateTime.Now;
            now = now.AddSeconds(-now.Second);

            /*todo
             * if (postType == null)
            {
                postType = _uow.PostTypeRepository.AsQueryable().FirstOrDefault(pt => pt.Id == _uow.LicenseManager.GetOptions(licenseId).GetInt("DefaultPostTypeId"));
            }*/

            return new Post
            {
                PostType = postType,
                PublishedAt = now,
                StartedAt = DateTime.Today.AddDays(1).AddHours(23),
                EndedAt = DateTime.Today.AddDays(2).AddHours(5),
                Published = true,
                Categories = new List<Category>()
            };
        }

        public void CreateAndSave(Post post, User user, PostType postType, IEnumerable<int> categoriesIds = null, Dictionary<string, string> fieldValues = null, Post parent = null)
        {
            if (post.PublishedAt == null) post.PublishedAt = DateTime.Now;
            post.CreatedAt = DateTime.Now;
                post.CreatedBy = user;

            if (postType != null)
            {
                post.PostType = postType;
            }
            post.FieldsValuesJson = InternalJsonSerializer.Serialize(fieldValues);
            post.Parent = parent;

            var slugCreator = new SlugCreator<Post>(_uow.PostRepository);
            post.Slug = slugCreator.Create(post);

            _uow.PostRepository.Add(post);
            _uow.Commit();
            SaveCategoriesToPost(post, categoriesIds);
        }

        public IQueryable<Post> GetByPostType(int postTypeId)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.PostType.Id == postTypeId)
                .OrderByDescending(p => p.PublishedAt);
        }

        public Post GetById(int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            return _uow.PostRepository.GetById(id.Value);
        }

        public Post GetByHash(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                return null;
            }
            return _uow.PostRepository.AsQueryable().FirstOrDefault(p=>p.Hash.ToLowerInvariant() == hash.ToLowerInvariant());
        }

        public void Change(Post post, User user, PostType postType = null, Post parent = null, IEnumerable<int> categoriesIds = null, Dictionary<string, string> fieldValues = null)
        {
            post.Categories.Clear();
            _uow.Commit();

            post.UpdatedAt = DateTime.Now;
            post.UpdatedBy = user;

            if (postType != null)
            {
                post.PostType = postType;
            }

            post.Parent = parent;
            post.FieldsValuesJson = InternalJsonSerializer.Serialize(fieldValues);

            SaveCategoriesToPost(post, categoriesIds);

            _uow.PostRepository.Edit(post);
            _uow.Commit();
        }

        public void Delete(int id)
        {
            using (var con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                using (var t = con.BeginTransaction())
                {
                    var command = new MySqlCommand(@"update post set createdby_id = null where id = @id;
                                                    update post set updatedby_id = null where id = @id;
                                                    update post set parent_Id = null where parent_id = @id;
                                                    delete from categoriestoposts where post_id = @id;
                                                    delete FROM post where id = @id", con);

                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    t.Commit();
                }
            }
        }

        public IEnumerable GetWithoutParentFromType(int postId, int postTypeId)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.PostType.Id == postTypeId && p.Id != postId && (p.Parent == null || p.Parent.Id != postId))
                .OrderBy(p => p.Title)
                .ToList();
        }

        //Refactor: Rancar connection strings daqui e talvez sqls
        private static void SaveCategoriesToPost(IHaveId post, IEnumerable<int> categoriesIds)
        {
            if (categoriesIds == null)
            {
                return;
            }
            using (var con = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                using (var t = con.BeginTransaction())
                {
                    var command =
                        new MySqlCommand(@"delete from categoriestoposts where Post_id=@id", con);
                    command.Parameters.AddWithValue("@id", post.Id);
                    command.ExecuteNonQuery();

                    if (categoriesIds != null)
                    {
                        foreach (var categoryId in categoriesIds)
                        {
                            var command2 =
                                new MySqlCommand(
                                    @"insert into categoriestoposts (Category_Id, Post_id) values (@categoryId,@postId)",
                                    con);
                            command2.Parameters.AddWithValue("@categoryId", categoryId);
                            command2.Parameters.AddWithValue("@postId", post.Id);
                            command2.ExecuteNonQuery();
                        }
                    }
                    t.Commit();
                }
            }
        }

        public IEnumerable<Post> GetFeatured()
        {
            return _uow.PostRepository.AsQueryable()
                .Where(p => p.Published && p.IsFeatured)
                .OrderBy(b => b.FeaturedOrder);
        }

        public IEnumerable<Post> GetWhatAllowsMarkAsFeatured()
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.Published && p.PostType != null && p.PostType.BehaviorStr.Contains("MarkAsFeatured"))
                .OrderByDescending(bp => bp.PublishedAt)
                .ThenBy(bp => bp.FeaturedOrder);
        }

        public void SetFeatureds(List<int> featuredPosts)
        {
            foreach (var post in _uow.PostRepository.AsQueryable().Where(p => p.IsFeatured))
            {
                post.IsFeatured = false;
                post.FeaturedOrder = 0;
                _uow.PostRepository.Edit(post);
            }
            if (featuredPosts != null)
            {
                for (var i = 0; i < featuredPosts.Count; i++)
                {
                    var postId = featuredPosts[i];
                    var post = _uow.PostRepository.GetById(postId);
                    if (post != null)
                    {
                        post.IsFeatured = true;
                        post.FeaturedOrder = i;
                    }
                    _uow.PostRepository.Edit(post);
                }
                _uow.Commit();
            }
        }

        public Post GetPublishedBySlugAndPostType(string slug, int postTypeId)
        {
            slug = slug.ToSlug();
            return _uow.PostRepository
                .AsQueryable()
                .FirstOrDefault(p => (p.Published && p.PostType.Id == postTypeId && p.Slug == slug || p.Hash.ToUpperInvariant() == slug.ToUpperInvariant()));
        }

        public IQueryable<Post> GetPublishedByPostType(int postTypeId)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.PostType.Id == postTypeId && p.Published);
        }

        public IQueryable<Post> GetPublishedByPostType(string singularName)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.PostType.SingularName == singularName && p.Published);
        }

        public IQueryable<Post> GetPublishedByPostTypeAfterDate(string singularName, DateTime date)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.PostType.SingularName == singularName && p.Published && p.StartedAt > date)
                .OrderBy(p => p.StartedAt);
        }

        public Post GetLastPublished()
        {
            return GetLastPublished(1).FirstOrDefault();
        }

        public IQueryable<Post> GetLastPublished(int amount)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(bp => bp.Published)
                .OrderByDescending(bp => bp.PublishedAt)
                .Take(amount);
        }

        public IQueryable<Post> GetPublishedByCategoryAndPostType(Category category, int postTypeId)
        {
            return _uow.PostRepository
                .AsQueryable()
                .Where(p => p.PostType.Id == postTypeId && p.Published && p.Categories.Any(c => c == category));
        }

        public Post GetByTitle(string title)
        {
            return _uow.PostRepository.AsQueryable().FirstOrDefault(p => p.Title == title);
        }

        public Post GetByExternalService(string externalServiceId, string externalServiceName)
        {
            return _uow.PostRepository
                .AsQueryable()
                .FirstOrDefault(p => p.ExternalServiceId == externalServiceId
                                     && p.ExternalServiceName == externalServiceName);
        }
    }
}
