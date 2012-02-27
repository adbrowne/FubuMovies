using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMovies.Web;
using FubuMovies.Web.Api;
using FubuMovies.Web.Timetable;
using FubuMVC.Core;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Security;
using FubuMVC.Core.Security.AntiForgery;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Core.UI.Extensibility;
using FubuMVC.Core.Urls;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using Enumerable = System.Linq.Enumerable;

namespace FubuMovies.FubuConfiguration
{
    public class FubuMoviesRegistry : FubuRegistry
    {
        public FubuMoviesRegistry()
        {
            IncludeDiagnostics(true);
            Applies
                .ToThisAssembly();

            Actions
                .IncludeClassesSuffixedWithController();

            Actions.IncludeType<ApiController<Movie>>();
            Actions.IncludeType<ApiController<MovieSession>>();

            Routes
                .HomeIs<TimetableRequest>()
                .IgnoreControllerNamespaceEntirely()
                .IgnoreMethodSuffix("Command")
                .IgnoreMethodSuffix("Query")
                .UrlPolicy(new MyUrlPolicy());

            //Actions.IncludeTypes(t => t.Name.EndsWith("Handler")).IgnoreMethodsDeclaredBy<AuthorizationHandler>();

            ApplyHandlerConventions<HandlersMarker>();

            Policies.Add<AntiForgeryPolicy>();

            Policies.WrapBehaviorChainsWith<TransactionBehavior>();

            Policies.WrapBehaviorChainsWith<LoginBehaviour>().Ordering(a => a.MustBeBeforeAuthorization());

            Configure(graph =>
                      graph.ApplyRedirectOnAddAndUpdate()
                );

            this.UseSpark();

            Views
                .TryToAttachWithDefaultConventions()
                .TryToAttachViewsInPackages()
                .RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));
            
            HtmlConvention<MyPasswordConvention>();
            HtmlConvention<MyLoginFormConvention>();
            Services(s => s.AddService(typeof(HtmlConventionRegistry), ObjectDef.ForType<EntityReferenceConvention>()));

            //HtmlConvention<EntityReferenceConvention>();
            HtmlConvention<DefaultHtmlConventions>();
            
            this.Validation(validation =>
                                {
                                    validation.Actions.Include(
                                        call =>
                                        call.HasInput &&
                                        Enumerable.Contains(call.InputType().GetInterfaces(), typeof(IValidationModel)));

                                    validation
                                        .Failures
                                        .If(call => call.InputType() != null &&
                                                    Enumerable.Contains(call.InputType().GetInterfaces(), typeof(IValidationModel)))
                                        .TransferBy<ValidationHandlerModelDescriptor>();
                                });

            Services(s =>
                         {
                             s.ReplaceService<IUrlTemplatePattern, JQueryUrlTemplate>();
                             s.AddService<IAuthorizationFailureHandler, AuthorizationHandler>();
                             s.AddService<IElementNamingConvention, DotNotationElementNamingConvention>();
                         });

            this.Extensions()
                .For<TimetableViewModel>("extension-placeholder", x => "<p>Rendered from content extension.</p>");
        }
    }
}