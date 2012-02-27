using FubuMovies.Infrastructure;

namespace FubuMovies.Web.Login
{
    public class PostHandler
    {
        private readonly IAuthenticationService authenticationService;

        public PostHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public LoginResultModel Execute(LoginSubmitInputModel input)
        {
            var authenticated = authenticationService.Authenticate(
                input.Username, 
                input.Password
            );

            return new LoginResultModel { Success = authenticated };
        }
    }

    public class LoginResultModel
    {
        public bool Success { get; set; }
    }
}