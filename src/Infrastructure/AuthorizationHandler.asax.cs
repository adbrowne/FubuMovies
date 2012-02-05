using FubuMovies.Login;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.Urls;

namespace FubuMovies.Infrastructure
{
    public class AuthorizationHandler : IAuthorizationFailureHandler
    {
        private readonly IOutputWriter _writer;
        private readonly IUrlRegistry _registry;

        public AuthorizationHandler(IOutputWriter writer, IUrlRegistry registry)
        {
            _writer = writer;
            _registry = registry;
        }

        public void Handle()
        {
            string url = _registry.UrlFor(new LoginIndexInputModel());
            _writer.RedirectToUrl(url);
        }
    }
}