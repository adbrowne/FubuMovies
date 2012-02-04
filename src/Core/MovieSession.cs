using System;

namespace FubuMovies.Core
{
    public class MovieSession
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime StartTime { get; set; }
    }
}