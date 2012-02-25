using System;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMovies.Web.Api;
using FubuMVC.Core.UI;
using FubuCore;
using FubuMVC.Core.UI.Configuration;
using HtmlTags;
using NHibernate;

namespace FubuMovies.FubuConfiguration
{
    public class EntityReferenceConvention : HtmlConventionRegistry
    {

        public EntityReferenceConvention()
        {
            //Editors.If(x => x.Accessor.Name == "Movie").BuildBy(builder =>
            //{
            //    return NewSelectList();
            //});
            Editors.Builder<MyBuilder>();
            //Editors.IfPropertyTypeIs(IsEntity).BuildBy(builder =>
            //                                                                    {
            //                                                                        return NewSelectList();
            //                                                                    });
        }

        private bool IsEntity(Type x)
        {
            return x.CanBeCastTo<IEntity>();
        }

        private HtmlTag NewSelectList()
        {
            var selectList = new SelectTag();
            return selectList;
            //return new HtmlTag("input").Attr("type", "password");
        }
    }

    public class MyBuilder : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return IsEntity(def.Accessor.PropertyType) && def.ModelType.CanBeCastTo <ISessionViewModel>();
        }

        private bool IsEntity(Type x)
        {
            return x.CanBeCastTo<IEntity>();
        }

        public override HtmlTag Build(ElementRequest request)
        {
            var session = ((ISessionViewModel) request.Model).Session;
            var items = session.CreateCriteria(request.Accessor.PropertyType).List<IEntity>();
            var selectList = new SelectTag();
            foreach (var entity in items)
            {
                selectList.Option(entity.ToString(), entity.Id);
            }
            return selectList;
        }
    }
}