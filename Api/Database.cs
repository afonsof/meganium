using System;
using DevTrends.MvcDonutCaching;
using NHibernate;

namespace Meganium.Api
{
    public class Database: IDisposable
    {
        private ITransaction _transaction;
        private readonly string _connectionString;
        private ISession _session;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ISession Session
        {
            get
            {
                if (_session != null) return _session;

                using (var sessionFactory = NHibernateBuilder.GetSessionFactory(_connectionString))
                {
                    _session = sessionFactory.OpenSession();
                }
                return _session;
            }
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = Session.BeginTransaction();
            }
        }

        public void Commit()
        {
            if (_transaction == null) return;
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems();
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            if (_session != null)
            {
                _session.Dispose();
            }
        }
    }
}