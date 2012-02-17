using System;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuValidation;

namespace FubuMovies.Web.Mapping
{
    public class MovieViewModel : IViewModel<Movie>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}