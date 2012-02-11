using System;

namespace FubuMovies.Core
{
    [Plural("Movies")]
    public class Movie
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

 
    }

    public class PluralAttribute : Attribute
    {
        public string Plural { get; set; }

        public PluralAttribute(string plural)
        {
            Plural = plural;
        }
    }
}