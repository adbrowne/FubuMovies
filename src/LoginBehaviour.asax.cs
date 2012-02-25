using System.Security.Principal;
using System.Web;
using FubuMVC.Core.Behaviors;

namespace FubuMovies
{
    public class LoginBehaviour : IActionBehavior
    {
        private readonly IActionBehavior innerBehaviour;

        public LoginBehaviour(IActionBehavior innerBehaviour)
        {
            this.innerBehaviour = innerBehaviour;
        }

        public void Invoke()
        {
            if ((string)HttpContext.Current.Session["user"] == "admin")
            {
                HttpContext.Current.User = new GenericPrincipal(new GenericIdentity("somebody"), new[] { "manager" });
            }
            innerBehaviour.Invoke();
        }

        public void InvokePartial()
        {
            innerBehaviour.InvokePartial();
        }
    }
}