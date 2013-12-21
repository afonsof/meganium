using System.Collections.Generic;
using System.Linq;
using Dongle.System;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Managers
{
    public class CategoryManager
    {
        private readonly IRepositories _uow;

        public CategoryManager(IRepositories uow)
        {
            _uow = uow;
        }

        public IEnumerable<Category> GetFromType(int postTypeId)
        {
            return _uow.CategoryRepository.AsQueryable().Where(c => c.PostType.Id == postTypeId).OrderBy(c => c.Title);
        }

        public object GetAll()
        {
            return _uow.CategoryRepository.AsQueryable().OrderBy(c => c.Title);
        }

        public Category GetById(int id)
        {
            return _uow.CategoryRepository.GetById(id);
        }

        public void Remove(int id)
        {
            var category = _uow.CategoryRepository.GetById(id);
            category.Posts = null;
            _uow.CategoryRepository.Remove(category);
            _uow.Commit();
        }

        public void Change(Category category, int postTypeId)
        {
            category.PostType = _uow.PostTypeRepository.GetById(postTypeId);
            _uow.CategoryRepository.Edit(category);
            _uow.Commit();
        }

        public Category Create()
        {
            return new Category();
        }

        public void CreateAndSave(Category category, int? postTypeId = null)
        {
            category.Slug = category.Title.ToSlug();
            if(postTypeId.HasValue) category.PostType = _uow.PostTypeRepository.GetById(postTypeId.Value);
            _uow.CategoryRepository.Add(category);
            _uow.Commit();
        }

        public Category GetByTitleAndPostType(string title, PostType postType)
        {
            return _uow.CategoryRepository
                .AsQueryable()
                .FirstOrDefault(c => c.Title == title && c.PostType.Id == postType.Id);
        }

        public Category GetBySlugAndPostType(string slug, int postTypeId)
        {
            slug = slug.ToSlug();
            return _uow.CategoryRepository
                .AsQueryable()
                .FirstOrDefault(c => c.Slug == slug && c.PostType.Id == postTypeId);
        }

         public IQueryable<Category> GetByPostType(string postTypeSingularName)
        {
            return _uow.CategoryRepository
                .AsQueryable()
                .Where(c => c.PostType.SingularName == postTypeSingularName);
        }
    }
}