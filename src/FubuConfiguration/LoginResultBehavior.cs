using FubuMovies.Web.Admin;
using FubuMovies.Web.Login;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;

namespace FubuMovies.FubuConfiguration
{
    public class LoginResultBehavior : IActionBehavior
    {
        private readonly IFubuRequest request;
        private readonly IUrlRegistry registry;
        private readonly IOutputWriter writer;
        private readonly IActionBehavior innerBehavior;
        private readonly IPartialFactory factory;

        public LoginResultBehavior(
            IFubuRequest request, 
            IUrlRegistry registry,
            IOutputWriter writer,
            IActionBehavior innerBehavior,
            IPartialFactory factory
        )
        {
            this.request = request;
            this.registry = registry;
            this.writer = writer;
            this.innerBehavior = innerBehavior;
            this.factory = factory;
        }

        public void Invoke()
        {
            innerBehavior.Invoke();
           
            var loginResult = request.Get<LoginResultModel>();
            if(loginResult.Success)
            {
                string url = registry.UrlFor<AdminInputModel>();
                writer.RedirectToUrl(url);
            }
            else
            {
                var inputModel = new LoginInputModel();
                request.SetObject(inputModel);

                IActionBehavior partial = factory.BuildPartial(inputModel.GetType());
                partial.InvokePartial();
            }
        }

        public void InvokePartial()
        {
            //innerBehavior.InvokePartial();           
        }
    }
}