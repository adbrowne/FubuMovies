using System.Web;
using FubuMovies.Admin;
using FubuMVC.Core.Continuations;
using FubuValidation;

namespace FubuMovies.Login
{
    public class LoginHandler
    {
        private readonly IAuthenticationService authenticationService;

        public LoginHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public LoginViewModel Get(LoginIndexInputModel indexInputModel)
        {
            return new LoginViewModel();
        }

        public FubuContinuation Post(LoginInputModel inputModel)
        {
            var username = inputModel.Username;
            var password = inputModel.Password;
            var authenticated = authenticationService.Authenticate(username, password);

            if (authenticated)
            {
                return FubuContinuation.RedirectTo(new EditorInputModel());
            }
            else
            {
                return FubuContinuation.TransferTo(new LoginIndexInputModel());
            }
        }

        
    }

    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }

    class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase httpContext;

        public AuthenticationService(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public bool Authenticate(string username, string password)
        {
            var authenticated = (username == "admin" && password == "admin");
            if(authenticated)
            {
                httpContext.Session["user"] = "admin";
            }
            return authenticated;
        }
    }

    public class LoginInputModel : IValidationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public interface IValidationModel
    {
    }

    public class LoginIndexInputModel
    {
    }

    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}