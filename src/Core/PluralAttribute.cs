using System;

namespace FubuMovies.Core
{
    public class PluralAttribute : Attribute
    {
        public string Plural { get; set; }

        public PluralAttribute(string plural)
        {
            Plural = plural;
        }
    }
}