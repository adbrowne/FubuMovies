using FubuMovies.Infrastructure;
using FubuValidation;

namespace FubuMovies.Web.Login
{
    public class LoginSubmitInputModel : IValidationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}