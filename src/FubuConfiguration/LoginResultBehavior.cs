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

        public LoginResultBehavior(
            IFubuRequest request, 
            IUrlRegistry registry,
            IOutputWriter writer,
            IActionBehavior innerBehavior
        )
        {
            this.request = request;
            this.registry = registry;
            this.writer = writer;
            this.innerBehavior = innerBehavior;
        }

        public void Invoke()
        {
            innerBehavior.Invoke();
           
            var loginResult = request.Get<LoginResultModel>();
            string url;
            if(loginResult.Success)
            {
                url = registry.UrlFor<AdminInputModel>();
            }
            else
            {
                url = registry.UrlFor<LoginInputModel>();
            }
            writer.RedirectToUrl(url);
        }

        public void InvokePartial()
        {
            innerBehavior.InvokePartial();           
        }
    }
}