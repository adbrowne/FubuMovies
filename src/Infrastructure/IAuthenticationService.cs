namespace FubuMovies.Login
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
    }
}