namespace FubuMovies.Infrastructure
{
    public interface IAuthenticationService
    {
        bool Authenticate(string username, string password);
        void Logout();
    }
}