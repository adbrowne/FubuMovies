using System;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMovies.Web.Api;
using NHibernate;

namespace FubuMovies.Web.Mapping
{
    public class MovieModelMapper : IModelMapper<Movie, MovieViewModel>
    {
        public MovieViewModel GetViewModel(Movie movie)
        {
            return new MovieViewModel
                       {
                           Name = movie.Name,
                           Description = movie.Description,
                           Id = movie.Id
                       };
        }

        public Movie GetEntity(MovieViewModel viewModel)
        {
            var movie = new Movie
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description
            };
            return movie;
        }
    }

    public class MovieSessionModelMapper : IModelMapper<MovieSession, MovieSessionViewModel>
    {
        private readonly ISession session;

        public MovieSessionModelMapper(ISession session)
        {
            this.session = session;
        }

        public MovieSessionViewModel GetViewModel(MovieSession movie)
        {
            return new MovieSessionViewModel
                       {
                           Id = movie.Id,
                           MovieId = movie.Movie.Id,
                           MovieName = movie.Movie.Name,
                           StartTime = movie.StartTime
                       };
        }

        public MovieSession GetEntity(MovieSessionViewModel viewModel)
        {
            return new MovieSession
                       {
                           Id = viewModel.Id,
                           StartTime = viewModel.StartTime,
                           Movie = session.Get<Movie>(viewModel.MovieId)
                       };
        }
    }

    public class MovieSessionViewModel : IViewModel<MovieSession>
    {
        public int MovieId { get; set; }

        public DateTime StartTime { get; set; }

        public string MovieName { get; set; }

        public int Id { get; set; }
    }
}