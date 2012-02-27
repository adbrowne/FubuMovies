using FubuMovies.Infrastructure;
using FubuMovies.Web.Admin;
using FubuMovies.Web.Admin.Editor;
using FubuMVC.Core.Continuations;

namespace FubuMovies.Web.Login
{
    public class PostHandler
    {
        private readonly IAuthenticationService authenticationService;

        public PostHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public FubuContinuation Execute(LoginSubmitInputModel input)
        {
            var authenticated = authenticationService.Authenticate(
                input.Username, 
                input.Password
            );

            if (authenticated)
            {
                return FubuContinuation.RedirectTo(new AdminInputModel());
            }
            else
            {
                return FubuContinuation.TransferTo(new LoginInputModel());
            }
        }

        
    }
}