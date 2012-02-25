using System;
using NHibernate;

namespace FubuMovies.Web.Api
{
    public class NewViewModel<T> : ISessionViewModel
    {
        public T Entity { get; set; }

        public ISession Session { get; set; }
    }
}