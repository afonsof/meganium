using System.Linq;
using MegaSite.Api.Entities;

namespace MegaSite.Api.Repositories
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