using FubuValidation;

namespace FubuMovies.Infrastructure
{
    public class LoginSubmitInputModel : IValidationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}