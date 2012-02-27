using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMVC.Core;
using NHibernate;
using NHibernate.Criterion;

namespace FubuMovies.Web.Api
{
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
            return new NewViewModel<TEntity>
                       {
                           Session = session
                       };
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
}
