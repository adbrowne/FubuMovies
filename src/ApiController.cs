using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuMovies.Infrastructure;
using FubuMVC.Core;
using NHibernate;

namespace FubuMovies
{
    [Conneg]
    public class ApiController<T> where T : class
    {
        private ISession session;

        public ApiController(IUnitOfWork unitOfWork)
        {
            session = unitOfWork.CurrentSession;
        }

        public ListViewModel<T> List(ListInputModel<T> input)
        {
            var listViewModel  = new ListViewModel<T>();
            listViewModel.AddRange(session.CreateCriteria<T>().SetFetchMode("Movie", FetchMode.Eager).List<T>().ToList());

            return listViewModel;
        }

        public AddViewModel<T> Add(T input)
        {
            session.Save(input);

            return new AddViewModel<T>();
        }
    }

    public class AddInputModel<T>
    {
        public T Entity { get; set; }
    }

    public class AddViewModel<T>
    {
    }

    public class ListInputModel<T>
    {
    }

    public class ListViewModel<T> : List<T>
    {
    }
}
