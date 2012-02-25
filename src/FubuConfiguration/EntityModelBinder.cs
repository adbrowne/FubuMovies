using System;
using FubuCore;
using FubuCore.Binding;
using FubuMovies.Core;

namespace FubuMovies.FubuConfiguration
{
    public class EntityModelBinder : IModelBinder
    {
        public bool Matches(Type type)
        {
            return type.CanBeCastTo<IEntity>();
        }

        public void Bind(Type type, object instance, IBindingContext context)
        {
            throw new NotImplementedException();
        }

        public object Bind(Type type, IBindingContext context)
        {
            var entity = context.ValueAs(type, "Id");

            if (entity != null) return entity;

            if (type.IsConcrete())
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}