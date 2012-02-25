using System;
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
    public class ApiController<TEntity> where TEntity : class, IEntity
    {
        private readonly ISession session;

        public ApiController(IUnitOfWork unitOfWork)
        {
            session = unitOfWork.CurrentSession;
        }

        public List<ViewModel<TEntity>> List(ListInputModel<TEntity> input)
        {
            var items = session.CreateCriteria<TEntity>().SetFetchMode("Movie", FetchMode.Join).List<TEntity>(); 
            return items.Select(x => new ViewModel<TEntity>{Entity = x}).ToList();
        }

        public NewViewModel<TEntity> New(NewInputModel<TEntity> input)
        {
            return new NewViewModel<TEntity>();
        }

        public GetViewModel<TEntity> Get(GetByIdInputModel<TEntity> input)
        {
            var item = session.CreateCriteria<TEntity>().Add(Restrictions.IdEq(input.Id)).UniqueResult<TEntity>();
            return new GetViewModel<TEntity> {Entity = item};
        }

        public ViewModel<TEntity> Update(UpdateModel<TEntity> input)
        {

            var entity = input.Entity;
            session.Update(entity);
            return new ViewModel<TEntity>{Entity = entity};
        }


        public ViewModel<TEntity> Add(AddModel<TEntity> input)
        {
            var entity = input.Entity;
            session.Save(entity);

            return new ViewModel<TEntity> {Entity = entity};
        }
    }

    public class UpdateModel<T> where T: IEntity
    {
        public T Entity { get; set; }

        
    }

    public class AddModel<T>
    {
        public T Entity { get; set; }

        
    }

    public class ViewModel<T>
    {
        public T Entity { get; set; }
    }


    public class GetViewModel<T>
    {
        public T Entity { get; set; }
    }

    public class NewViewModel<T>
    {
    }

    public class NewInputModel<T>
    {
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
