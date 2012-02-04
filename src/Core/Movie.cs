using System;

namespace FubuMovies.Core
{
    public class Movie
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}