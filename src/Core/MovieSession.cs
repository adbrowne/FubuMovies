using System;

namespace FubuMovies.Core
{
    public class MovieSession
    {
        public virtual int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual DateTime StartTime { get; set; }
    }
}