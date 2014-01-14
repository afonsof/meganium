using System.Linq;
using Meganium.Api.Entities;
using NHibernate.Linq;

namespace Meganium.Api.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IHaveId
    {
        private readonly Database _database;

        public Repository(Database database)
        {
            _database = database;
        }

        public TEntity GetById(int id)
        {
            return _database.Session.Query<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        public virtual void Add(TEntity obj)
        {
            _database.BeginTransaction();
            _database.Session.Save(obj);
        }

        public void Remove(TEntity obj)
        {
            _database.BeginTransaction();
            _database.Session.Delete(obj);
        }

        public virtual void Edit(TEntity obj)
        {
            _database.BeginTransaction();
            _database.Session.Update(obj);
        }

        public void Remove(int id)
        {
            Remove(GetById(id));
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _database.Session.Query<TEntity>();
        }
    }
}