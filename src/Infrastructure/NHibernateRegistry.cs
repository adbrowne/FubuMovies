using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FubuMovies.Core;
using FubuMovies.Infrastructure.Maps;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using Environment = NHibernate.Cfg.Environment;


namespace FubuMovies.Infrastructure
{

    public class NHibernateRegistry : Registry
    {
        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
              .Database(
                MsSqlConfiguration.MsSql2008
                .ConnectionString(c => c.FromConnectionStringWithKey("MovieDB"))
              )
              .Mappings(m =>
                m.FluentMappings.AddFromAssemblyOf<MovieSessionMap>())
              .BuildSessionFactory();
        }

        public NHibernateRegistry()
        {
            var cfg = new Configuration()
                .SetProperty(Environment.ReleaseConnections, "on_close")
                .SetProperty(Environment.Dialect, typeof(MsSql2005Dialect).AssemblyQualifiedName)
                .SetProperty(Environment.ConnectionDriver, typeof(SqlClientDriver).AssemblyQualifiedName)
                .SetProperty(Environment.ConnectionStringName, "MovieDB")
                .SetProperty(Environment.ProxyFactoryFactoryClass, typeof(ProxyFactoryFactory).AssemblyQualifiedName)
                .AddAssembly(typeof(MovieSession).Assembly);

            var sessionFactory = CreateSessionFactory();

            For<Configuration>().Singleton().Use(cfg);

            For<ISessionFactory>().Singleton().Use(sessionFactory);

            For<ISession>().LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.Hybrid))
                .Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());

            For<IUnitOfWork>().LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.Hybrid))
                .Use<UnitOfWork>();

            //ForRequestedType<IDatabaseBuilder>().TheDefaultIsConcreteType<DatabaseBuilder>();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        ISession CurrentSession { get; }
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory sessionFactory;
        private readonly ITransaction transaction;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
            CurrentSession = this.sessionFactory.OpenSession();
            transaction = CurrentSession.BeginTransaction();
        }

        public ISession CurrentSession { get; private set; }

        public void Dispose()
        {
            CurrentSession.Close();
            CurrentSession = null;
        }

        public void Commit()
        {
            transaction.Commit();
        }
    }
}