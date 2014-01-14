using System.Linq;
using Meganium.Api.Entities;

namespace Meganium.Api.Repositories
{
    public interface IRepository<TEntity> where TEntity : IHaveId
    {
        TEntity GetById(int id);
        IQueryable<TEntity> AsQueryable();

        void Add(TEntity obj);
        void Edit(TEntity obj);
        void Remove(TEntity obj);
        void Remove(int id);
    }
}