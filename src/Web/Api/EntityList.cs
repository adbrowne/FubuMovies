using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FubuMovies.Core;

namespace FubuMovies.Web.Api
{
    // Cludgy workaround because FubuMVC.Spark.Registration.GenericParser doesn't seem to support nested generics
    public class EntityList<T> : IEnumerable<ViewModel<T>> where T : IEntity, new()
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
}