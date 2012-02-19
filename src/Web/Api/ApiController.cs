using System.Collections.Generic;
using System.Linq;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMVC.Core;
using FubuMVC.Core.View;
using NHibernate;
using NHibernate.Criterion;

namespace FubuMovies.Web.Api
{
    public interface IViewModel
    {
        int Id { get; set; }
    }
    public static class ViewHelpers
    {
        public static GetByIdInputModel<TViewModel> GetInputModel<TViewModel>(this IFubuPage page, TViewModel model) where TViewModel : IViewModel
        {
            var getByIdInputModel = new GetByIdInputModel<TViewModel>
                                        {
                                            Id = model.Id
                                        };
            return getByIdInputModel;
        }

        public static TUpdateModel GetUpdateModel<TViewModel, TUpdateModel>(this IFubuPage page, TViewModel model) where TViewModel : IViewModel where TUpdateModel : IViewModel, new()
        {
           return new TUpdateModel
                      {
                          Id = model.Id
                      };
        }
    }
    [Conneg(FormatterOptions.Json | FormatterOptions.Html)]
    public class ApiController<TEntity, TViewModel, TUpdateModel> where TEntity : class, IEntity where TViewModel: IViewModel where TUpdateModel : TViewModel
    {
        private readonly IModelMapper<TEntity, TViewModel> mapper;
        private readonly ISession session;

        public ApiController(IUnitOfWork unitOfWork, IModelMapper<TEntity, TViewModel> mapper)
        {
            this.mapper = mapper;
            session = unitOfWork.CurrentSession;
        }

        public List<TViewModel> List(ListInputModel<TEntity> input)
        {
            var items = session.CreateCriteria<TEntity>().List<TEntity>();
            return items.Select(x => mapper.GetViewModel(x)).ToList();
        }

        public TViewModel Get(GetByIdInputModel<TViewModel> input)
        {
            var item = session.CreateCriteria<TEntity>().Add(Restrictions.IdEq(input.Id)).UniqueResult<TEntity>();
            return mapper.GetViewModel(item);
        }

        public TViewModel Update(TUpdateModel input)
        {
            //var item = session.CreateCriteria<TEntity>().Add(Restrictions.IdEq(input.Id)).UniqueResult<TEntity>();

            var entity = mapper.GetEntity(input);
            session.Update(entity);
            return mapper.GetViewModel(entity);
        }


        public TViewModel Add(TViewModel input)
        {
            var entity = mapper.GetEntity(input);
            session.Save(entity);

            return mapper.GetViewModel(entity);
        }
    }

    public class GetByIdInputModel<TEntity>
    {
        public int Id { get; set; }
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
