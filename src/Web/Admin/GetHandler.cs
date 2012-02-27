using FubuMovies.Core;

namespace FubuMovies.Web.Admin
{
    public class GetHandler
    {
        public AdminViewModel Execute(AdminInputModel input)
        {
            return new AdminViewModel();
        }
    }

    public class AdminInputModel
    {
    }

    public class AdminViewModel
    {
        public object SessionList
        {
            get { return new Api.ListInputModel<MovieSession>(); }
        }
        public object MovieList
        {
            get { return new Api.ListInputModel<Movie>(); }
        }
    }
}