using System;
using System.Globalization;
using MegaSite.Api.Entities;
using MegaSite.Api.Repositories;
using Dongle.System;
using System.Linq;

namespace MegaSite.Api.Trash
{
    public class SlugCreator<TEntity> where TEntity : IHaveSlug, IHaveId
    {
        private readonly IRepository<TEntity> _repositoryReader;

        public SlugCreator(IRepository<TEntity> repositoryReader)
        {
            _repositoryReader = repositoryReader;
        }

        public string Create(TEntity obj, string wantedName = "")
        {
            var baseSlug = !string.IsNullOrEmpty(wantedName) ? wantedName : (!string.IsNullOrEmpty(obj.Title) ? obj.Title.ToSlug() : new Random().Next(Int32.MaxValue).ToString(CultureInfo.InvariantCulture));
            var slug = baseSlug;
            var count = 0;
            while (_repositoryReader.AsQueryable().Any(p => p.Slug == slug))
            {
                count++;
                slug = baseSlug + "-" + count;
            }
            return slug;
        }
    }
}