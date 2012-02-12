using System;

namespace FubuMovies.Core
{
    [Plural("MovieSessions")]
    public class MovieSession : IEntity
    {
        public virtual int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual DateTime StartTime { get; set; }
    }
}