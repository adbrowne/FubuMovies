using FubuMovies.Core;

namespace FubuMovies.Infrastructure.Maps
{
    public class MovieMap : FluentNHibernate.Mapping.ClassMap<Movie>
    {
        public MovieMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description);
        }
    }
}