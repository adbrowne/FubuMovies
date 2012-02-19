using FubuMovies.Admin;
using FubuMovies.Infrastructure;
using FubuMVC.Core.Continuations;

namespace FubuMovies.Login
{
    public class PostHandler
    {
        private readonly IAuthenticationService authenticationService;

        public PostHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public FubuContinuation Post(LoginSubmitInputModel input)
        {
            var authenticated = authenticationService.Authenticate(
                input.Username, 
                input.Password
            );

            if (authenticated)
            {
                return FubuContinuation.RedirectTo(new EditorInputModel());
            }
            else
            {
                return FubuContinuation.TransferTo(new LoginInputModel());
            }
        }

        
    }
}