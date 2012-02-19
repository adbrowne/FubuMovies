using FubuMovies.Admin;
using FubuMVC.Core.Continuations;
using FubuValidation;

namespace FubuMovies.Login
{
    public class PostHandler
    {
        private readonly IAuthenticationService authenticationService;

        public PostHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
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