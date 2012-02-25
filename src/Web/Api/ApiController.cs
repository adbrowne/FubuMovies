using System;
using System.Collections;
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
        public static GetByIdInputModel<TEntity> GetInputModel<TEntity>(this IFubuPage page, TEntity entity) where TEntity : IEntity
        {
            var getByIdInputModel = new GetByIdInputModel<TEntity>
                                        {
                                            Id = entity.Id
                                        };
            return getByIdInputModel;
        }

        public static UpdateModel<TEntity> GetUpdateModel<TEntity>(this IFubuPage page, TEntity entity) where TEntity: IEntity, new()
        {
           return new UpdateModel<TEntity>
                      {
                          Entity = new TEntity
                                       {
                                           Id = entity.Id
                                       }
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

        public EntityList<TEntity> List(ListInputModel<TEntity> input) 
        {
            var items = session.CreateCriteria<TEntity>().SetFetchMode("Movie", FetchMode.Join).List<TEntity>();
            return new EntityList<TEntity>(items);
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

    // Cludgy workaround because FubuMVC.Spark.Registration.GenericParser doesn't seem to support nested generics
    public class EntityList<T> : IEnumerable<ViewModel<T>> where T : IEntity
    {
        private readonly List<ViewModel<T>> internalList;

        public EntityList(IEnumerable<T> items)
        {
            internalList = new List<ViewModel<T>>();
            internalList.AddRange(items.Select(x => new ViewModel<T>{Entity = x}));
        }

        public EntityList() : this (new List<T>())
        {
            
        }

        public IEnumerator<ViewModel<T>> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class UpdateModel<T> where T: IEntity
    {
        public T Entity { get; set; }
        public int Id { 
            get { return Entity.Id; }
            set { Entity.Id = value; }
        }
    }

    public class AddModel<T>
    {
        public T Entity { get; set; }
    }

    public class ViewModel<T> : IViewModel where T : IEntity
    {
        public T Entity { get; set; }

        public int Id
        {
            get { return Entity.Id; }
            set { Entity.Id = value; }
        }
    }


    public class GetViewModel<T>
    {
        public T Entity { get; set; }
    }

    public class NewViewModel<T>
    {
        public T Entity { get; set; }
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
