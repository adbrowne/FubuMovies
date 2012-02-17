using System.Collections.Generic;
using System.Linq;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMVC.Core;
using NHibernate;

namespace FubuMovies
{
    [Conneg]
    public class ApiController<TEntity, TViewModel> where TEntity : class, IEntity
    {
        private readonly IModelMapper<TEntity, TViewModel> mapper;
        private readonly ISession session;

        public ApiController(IUnitOfWork unitOfWork, IModelMapper<TEntity, TViewModel> mapper)
        {
            this.mapper = mapper;
            session = unitOfWork.CurrentSession;
        }

        public IEnumerable<TViewModel> List(ListInputModel<TEntity> input)
        {
            var items = session.CreateCriteria<TEntity>().List<TEntity>();
            return items.Select(x => mapper.GetViewModel(x));
        }

        public TViewModel Add(TViewModel input)
        {
            var entity = mapper.GetEntity(input);
            session.Save(entity);

            return mapper.GetViewModel(entity);
        }
    }

    public interface IModelMapper<TEntity, TViewModel>
    {
        TViewModel GetViewModel(TEntity entity);
        TEntity GetEntity(TViewModel viewModel);
    }

    public class AddInputModel<T> : JsonMessage
        where T: IEntity
    {
        public T Entity { get; set; }
    }

    public class ListInputModel<T>
    {
    }

    public class ListViewModel<T> : List<T>
    {
    }
}
