using FubuMovies.Core;
using NHibernate;

namespace FubuMovies.Web.Api
{
    public class ViewModel<T> : ISessionViewModel, IViewModel<T> where T : IEntity
    {
        public T Entity { get; set; }

        public ISession Session {get;set;}

        public int Id
        {
            get { return Entity.Id; }
            set { Entity.Id = value; }
        }
    }
}