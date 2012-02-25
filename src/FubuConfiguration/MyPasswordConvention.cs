using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Configuration;
using HtmlTags;

namespace FubuMovies.FubuConfiguration
{
    public class MyPasswordConvention : HtmlConventionRegistry
    {
        public MyPasswordConvention()
        {
            Editors
                .If(x => IsPassword(x))
                .BuildBy(
                    builder => NewPasswordElement()
                );
        }

        private bool IsPassword(AccessorDef x)
        {
            return x.Accessor.Name == "Password";
        }

        private static HtmlTag NewPasswordElement()
        {
            return new HtmlTag("input").Attr("type", "password");
        }
    }
}