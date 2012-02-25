using FubuMovies.Core;

namespace FubuMovies.Web.Api
{
    public class ViewModel<T> : IViewModel<T> where T : IEntity
    {
        public T Entity { get; set; }

        public int Id
        {
            get { return Entity.Id; }
            set { Entity.Id = value; }
        }
    }
}