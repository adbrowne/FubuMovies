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
}