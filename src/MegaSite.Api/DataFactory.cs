using System;
using System.Configuration;
using MegaSite.Api.Repositories;

namespace MegaSite.Api
{
    public abstract class DataFactory
    {
        public IRepositories CreateInstance()
        {
            return new UnitOfWork(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }
    }
}