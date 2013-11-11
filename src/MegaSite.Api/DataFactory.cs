using System;
using System.Configuration;

namespace MegaSite.Api
{
    public abstract class DataFactory
    {
        public IUnitOfWork CreateInstance()
        {
            return new UnitOfWork(new Lazy<AuthenticatedUser>(), ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
    }
}