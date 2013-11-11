using System.Linq;
using MegaSite.Api.Entities;
using NHibernate.Linq;

namespace MegaSite.Api.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IHaveId
    {
        protected readonly UnitOfWork Uow;

        public Repository(UnitOfWork uow)
        {
            Uow = uow;
        }

        public TEntity GetById(int id)
        {
            return Uow.Session.Query<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        public virtual void Add(TEntity obj)
        {
            Uow.BeginTransaction();
            Uow.Session.Save(obj);
        }

        public void Remove(TEntity obj)
        {
            Uow.BeginTransaction();
            Uow.Session.Delete(obj);
        }

        public virtual void Edit(TEntity obj)
        {
            Uow.BeginTransaction();
            Uow.Session.Update(obj);
        }

        public void Remove(int id)
        {
            Remove(GetById(id));
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return Uow.Session.Query<TEntity>();
        }
    }
}