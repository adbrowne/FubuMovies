using FubuMovies.Login;
using FubuMVC.Core.UI;

namespace FubuMovies.FubuConfiguration
{
    public class MyLoginFormConvention : HtmlConventionRegistry
    {
        public MyLoginFormConvention()
        {
            Editors.If(x => x.ModelType.Equals(typeof(LoginViewModel))).AddClass("login");
        }
    }
}