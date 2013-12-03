using System;
using System.Configuration;

namespace MegaSite.Api
{
    public abstract class DataFactory
    {
        public IUnitOfWork CreateInstance()
        {
            return new UnitOfWork(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
    }
}