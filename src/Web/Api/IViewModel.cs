using NHibernate;

namespace FubuMovies.Web.Api
{
    public interface ISessionViewModel
    {
        ISession Session { get; }
    }
    public interface IViewModel<out T>
    {
        int Id { get; set; }
        T Entity { get; }
    }
}