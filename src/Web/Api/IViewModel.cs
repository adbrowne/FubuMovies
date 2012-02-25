namespace FubuMovies.Web.Api
{
    public interface IViewModel<out T>
    {
        int Id { get; set; }
        T Entity { get; }
    }
}