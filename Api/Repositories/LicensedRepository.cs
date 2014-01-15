using System.Linq;
using Meganium.Api.Entities;
using Meganium.Api.Managers;
using NHibernate.Linq;

namespace Meganium.Api.Repositories
{
    public class LicensedRepository<TEntity> : IRepository<TEntity> where TEntity : IHaveId, IHaveLicense
    {
        private readonly Database _database;
        private readonly IManagers _managers;

        public LicensedRepository(Database database, IManagers managers)
        {
            _database = database;
            _managers = managers;
        }

        public TEntity GetById(int id)
        {
            return _database.Session.Query<TEntity>().FirstOrDefault(e => e.Id == id && e.License.Id == _managers.License.Id);
        }

        public virtual void Add(TEntity obj)
        {
            obj.License = _database.Session.Get<License>(_managers.License.Id);
            _database.BeginTransaction();
            _database.Session.Save(obj);
        }

        public void Remove(TEntity obj)
        {
            _database.BeginTransaction();
            var command = _database.Session.Connection.CreateCommand();
            command.CommandText = string.Format("DELETE FROM {0} WHERE ID={1}", typeof(TEntity).Name, obj.Id);
            command.ExecuteNonQuery();
        }

        public virtual void Edit(TEntity obj)
        {
            obj.License = _database.Session.Get<License>(_managers.License.Id);
            _database.BeginTransaction();
            _database.Session.Update(obj);
        }

        public void Remove(int id)
        {
            Remove(GetById(id));
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _database.Session.Query<TEntity>().Where(e => e.License.Id == _managers.License.Id);
        }
    }
}