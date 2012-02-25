using FubuMovies.Core;
using FubuMVC.Core;

namespace FubuMovies.Web.Api
{
    public class AddInputModel<T> : JsonMessage
        where T: IEntity
    {
        public T Entity { get; set; }
    }
}