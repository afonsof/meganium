using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using MegaSite.Api.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;

namespace MegaSite.Api
{
    //TODO: Repensar neste cara aqui
    public static class NHibernateBuilder
    {
        public static void Reset(string connectionString)
        {
            GetConfig(connectionString)
                .ExposeConfiguration(BuildSchema)
                .BuildConfiguration();
        }

        public static ISessionFactory GetSessionFactory(string connectionString)
        {
            return GetConfig(connectionString)
                .BuildSessionFactory();
        }

        private static FluentConfiguration GetConfig(string connectionString)
        {
            var autoMap = AutoMap.AssemblyOf<PostType>().Where(type => typeof(IHaveId).IsAssignableFrom(type));
            autoMap.Conventions.Add(DefaultCascade.All());
            autoMap.OverrideAll(map => map.IgnoreProperties(x => x.CanWrite == false));

            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.AutoMappings.Add(autoMap));
        }

        private static void BuildSchema(Configuration config)
        {
            var nhvc = new NHibernate.Validator.Cfg.Loquacious.FluentConfiguration();
            var validator = new ValidatorEngine();
            validator.Configure(nhvc);
            config.Initialize(validator);

            var schemaExport = new SchemaExport(config);

            schemaExport.Drop(true, true);
            schemaExport.Create(true, true);
        }
    }
}