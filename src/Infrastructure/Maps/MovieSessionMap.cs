using FubuMovies.Core;

namespace FubuMovies.Infrastructure.Maps
{
    public class MovieSessionMap : FluentNHibernate.Mapping.ClassMap<MovieSession>
    {
        public MovieSessionMap()
        {
            Id(x => x.Id);
            References(x => x.Movie);
            Map(x => x.StartTime);
        }
    }
    public class MovieMap : FluentNHibernate.Mapping.ClassMap<Movie>
    {
        public MovieMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}