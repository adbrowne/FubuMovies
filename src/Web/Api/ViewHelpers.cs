﻿using FubuMovies.Core;
using FubuMVC.Core.View;

namespace FubuMovies.Web.Api
{
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
}