using System;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMovies.Web.Api;
using FubuValidation;

namespace FubuMovies.Web.Mapping
{
    public class MovieViewModel : IViewModel<Movie>, IViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class MovieUpdateModel : MovieViewModel { }
}