using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FubuMovies.Infrastructure.Maps;
using NHibernate;
using StructureMap;

namespace FubuMovies.Infrastructure
{
    public class NHibernateConfiguration
    {
        private static ISessionFactory ConfigureNHibernate(IPersistenceConfigurer databaseConfigurer, NHibernate.Cfg.Configuration cfg)
        {
            var factory =  Fluently.Configure(cfg)
                .Database(databaseConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<MovieSessionMap>())
                .ExposeConfiguration(c =>
                                         {
                                             c.SetProperty("adonet.batch_size", "5");
                                             c.SetProperty("generate_statistics", "true");
                                         })
                .BuildSessionFactory();

            return factory;
        }

        public static void ConfigureDataAccess(string sqlServerAddress, string username,
                                               string password, string database, InstanceScope sessionScope)
        {
            ObjectFactory.Initialize(i => ConfigureDataAccess(i, sqlServerAddress, username, password, database, sessionScope));
            ObjectFactory.AssertConfigurationIsValid();
        }

        public static void ConfigureDataAccess(IPersistenceConfigurer databaseConfigurer, InstanceScope sessionScope, out NHibernate.Cfg.Configuration cfg)
        {
            NHibernate.Cfg.Configuration configuration = null;
            ObjectFactory.Initialize(i => ConfigureDataAccess(i, databaseConfigurer, sessionScope, out configuration));
            ObjectFactory.AssertConfigurationIsValid();
            cfg = configuration;
        }

        public static void ConfigureDataAccess(IInitializationExpression i, string sqlServerAddress, string username,
                                               string password, string database, InstanceScope sessionScope)
        {
            NHibernate.Cfg.Configuration cfg;
            ConfigureDataAccess(i, MsSqlConfiguration.MsSql2005.ConnectionString(c => 
                c.Server(sqlServerAddress).Username(username).Password(password).Database(database)),sessionScope, out cfg);
        }

        public static void ConfigureDataAccess(IInitializationExpression i, IPersistenceConfigurer databaseConfigurer, InstanceScope sessionScope, out NHibernate.Cfg.Configuration cfg)
        {
            cfg = new NHibernate.Cfg.Configuration();

            i.ForRequestedType<ISessionFactory>()
                .CacheBy(InstanceScope.Singleton)
                .TheDefault.IsThis(ConfigureNHibernate(databaseConfigurer, cfg));

            i.ForRequestedType<ISession>()
                .CacheBy(sessionScope)
                .TheDefault.Is.ConstructedBy(() =>
                                             ObjectFactory.GetInstance<ISessionFactory>
                                                 ().OpenSession());




            //More StructureMap configuration goes here:

            //i.ForRequestedType<ISomeModelRepository>()
            //    .CacheBy(InstanceScope.PerRequest)
            //    .TheDefaultIsConcreteType<SomeModelRepository>();



        }
    }
}