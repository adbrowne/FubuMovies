using System.Security.Principal;
using System.Web;
using FubuMovies.Admin;
using FubuMovies.Timetable;
using FubuMVC.Core.Continuations;
using FubuValidation;

namespace FubuMovies.Login
{
    public class LoginHandler
    {
        public LoginViewModel Get(LoginIndexInputModel indexInputModel)
        {
            return new LoginViewModel();
        }

        public FubuContinuation Post(LoginInputModel inputModel)
        {
            var authenticated = (inputModel.Username == "admin" && inputModel.Password == "admin");

            if (authenticated)
            {
                HttpContext.Current.Session["user"] = "admin";
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