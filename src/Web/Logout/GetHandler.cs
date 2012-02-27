using FubuMovies.Infrastructure;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Security;

namespace FubuMovies.Web.Logout
{
    [AllowRole("manager")]
    public class GetHandler
    {
        private readonly IAuthenticationService authenticationService;

        public GetHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public FubuContinuation Execute(LogoutInputModel input)
        {
            authenticationService.Logout();
            return FubuContinuation.RedirectTo<Timetable.TimetableRequest>();
        }
    }

    public class LogoutInputModel
    {
    }
}