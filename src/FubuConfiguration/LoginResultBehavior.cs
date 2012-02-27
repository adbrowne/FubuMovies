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

        public LoginResultBehavior(
            IFubuRequest request, 
            IUrlRegistry registry,
            IOutputWriter writer
        )
        {
            this.request = request;
            this.registry = registry;
            this.writer = writer;
        }

        public void Invoke()
        {
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
            
        }
    }
}