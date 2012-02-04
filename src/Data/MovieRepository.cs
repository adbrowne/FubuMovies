using FubuMovies.Core;
using NHibernate;

namespace FubuMovies.Data
{
    public class MovieRepository
    {
        private readonly ISession _session;

        public MovieRepository(ISession session)
        {
            _session = session;
        }

        public void Save(Movie movie)
        {
            _session.Save(movie);
        }
    }
}