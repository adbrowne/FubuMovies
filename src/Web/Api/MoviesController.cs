using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMVC.Core;
using NHibernate;

namespace FubuMovies.Api
{
    public class MoviesController
    {
        private readonly ISession _session;

        public MoviesController(IUnitOfWork unitOfWork)
        {
            _session = unitOfWork.CurrentSession;
        }

        [Conneg]
        public MovieListViewModel List(MovieListInputModel input)
        {
            var movieListViewModel = new MovieListViewModel();
            movieListViewModel.AddRange(_session.CreateCriteria<Movie>().List<Movie>().ToList());
        
            return movieListViewModel;
        }

        public MovieViewModel Get(MovieGetInputModel input)
        {
            var movie = _session.Get<Movie>(input.Id);

            return new MovieViewModel
                       {
                           Id = movie.Id,
                           Name = movie.Name,
                           Description = movie.Description
                       };
        }

        public MovieUpdateViewModel Update(MovieUpdateInputModel input)
        {
            var movie = _session.Get<Movie>(input.Id);
            movie.Name = input.Name;
            movie.Description = input.Description;
            _session.Update(movie);
            _session.Flush();
            return new MovieUpdateViewModel();
        }

        public MovieNewViewModel New(MovieNewInputModel input)
        {
            return new MovieNewViewModel();
        }

        public MovieCreateViewModel Create(MovieCreateInputModel input)
        {
            var movie = new Movie {Name = input.Name, Description = input.Description};

            _session.Save(movie);

            return new MovieCreateViewModel();
        }
    }

    public class MovieNewInputModel
    {
        
    }

    public class MovieNewViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MovieCreateInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MovieCreateViewModel
    {
    }

    public class MovieUpdateInputModel : BaseMovieModelWithId
    {
    }

    public class MovieUpdateViewModel
    {
    }

    public class MovieGetInputModel
    {
        public int Id { get; set; }
    }

    public class BaseMovieModel
    {
        
        public string Description { get; set; }

        public string Name { get; set; }
    }

    public class BaseMovieModelWithId : BaseMovieModel
    {
        public int Id { get; set; }

    }

    public class MovieViewModel :BaseMovieModelWithId
    {
    }

    public class MovieListInputModel
    {
    }

    public class MovieListViewModel : List<Movie>
    {
        
    }
}