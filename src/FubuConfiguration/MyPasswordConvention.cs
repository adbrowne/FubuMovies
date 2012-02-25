using FubuMVC.Core.UI;
using HtmlTags;

namespace FubuMovies.FubuConfiguration
{
    public class MyPasswordConvention : HtmlConventionRegistry
    {
        public MyPasswordConvention()
        {
            Editors
                .If(x => x.Accessor.Name == "Password")
                .BuildBy(
                    builder => NewPasswordElement()
                );
        }

        private static HtmlTag NewPasswordElement()
        {
            return new HtmlTag("input").Attr("type", "password");
        }
    }
}