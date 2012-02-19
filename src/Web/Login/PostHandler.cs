using FubuMovies.Admin;
using FubuMVC.Core.Continuations;

namespace FubuMovies.Infrastructure
{
    public class PostHandler
    {
        private readonly IAuthenticationService authenticationService;

        public PostHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        

        public FubuContinuation Post(LoginSubmitInputModel submitInputModel)
        {
            var username = submitInputModel.Username;
            var password = submitInputModel.Password;
            var authenticated = authenticationService.Authenticate(username, password);

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