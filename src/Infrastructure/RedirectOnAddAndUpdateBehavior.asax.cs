using System.Linq;
using FubuMovies.Core;
using FubuMovies.Web.Api;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using FubuCore;
namespace FubuMovies.Infrastructure
{
    public class RedirectOnAddAndUpdateBehavior<T> : BasicBehavior where T: IEntity 
    {
        private readonly IFubuRequest request;
        private readonly IUrlRegistry registry;
        private readonly IOutputWriter writer;

        public RedirectOnAddAndUpdateBehavior(IFubuRequest request, IUrlRegistry registry,
            IOutputWriter writer)
            : base(PartialBehavior.Ignored)
        {
            this.request = request;
            this.registry = registry;
            this.writer = writer;
        }

        protected override DoNext performInvoke()
        {
            var mimeTypes = request.Get<CurrentMimeType>();
            // this should be changed to match the logic in ConnegOutputBehavior
            var acceptsHtml = mimeTypes.AcceptsHtml();

            if(acceptsHtml)
            {
                var outputModel = request.Get<ViewModel<T>>();
                //var outputModel = (IViewModel<IEntity>) request.Get(typeof (IViewModel<IEntity>));
                var inputModel = typeof(GetByIdInputModel<>).CloseAndBuildAs<GetByIdInputModel<T>>(new []{typeof(T)});
                inputModel.Id = outputModel.Id;
                var url = registry.UrlFor(inputModel);
                writer.RedirectToUrl(url);
                return DoNext.Stop;
            }
            else
            {
                return DoNext.Continue;
            }
        }
    }
}