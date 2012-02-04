using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using FubuMovies.Infrastructure;
using FubuMovies.Login;
using FubuMovies.Timetable;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.Security.AntiForgery;
using FubuMVC.Core.UI.Extensibility;
using FubuMVC.Core.Urls;
using FubuMVC.Spark;
using FubuMVC.StructureMap;
using StructureMap;

namespace FubuMovies
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication
                .For<FubuMoviesRegistry>()
                .StructureMap(() => new Container(new NHibernateRegistry()))
                .Bootstrap();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

    public class FubuMoviesRegistry : FubuRegistry
    {
        public FubuMoviesRegistry()
        {
            Applies
                .ToThisAssembly();

            Actions
                .IncludeClassesSuffixedWithController();

            ApplyHandlerConventions();

            Routes
                .HomeIs<TimetableRequest>()
                .IgnoreControllerNamespaceEntirely()
                .IgnoreMethodSuffix("Command")
                .IgnoreMethodSuffix("Query")
                .ConstrainToHttpMethod(action => action.Method.Name.EndsWith("Command"), "POST")
                .ConstrainToHttpMethod(action => action.Method.Name.StartsWith("Query"), "GET");

            Policies.Add<AntiForgeryPolicy>();

            Policies.WrapBehaviorChainsWith<TransactionBehavior>(); 


            this.UseSpark();

            Views
                .TryToAttachWithDefaultConventions()
                .TryToAttachViewsInPackages();

            //HtmlConvention<SampleHtmlConventions>();

            Services(s =>
            {
                //s.FillType<IExceptionHandler, AsyncExceptionHandler>();
                s.ReplaceService<IUrlTemplatePattern, JQueryUrlTemplate>();
                s.AddService<IAuthorizationFailureHandler, AuthorizationHandler>();
            });

            this.Extensions()
                .For<Timetable.TimetableViewModel>("extension-placeholder", x => "<p>Rendered from content extension.</p>");
        }
    }

    class TransactionBehavior : IActionBehavior
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActionBehavior _innerBehaviour;

        public TransactionBehavior(IUnitOfWork unitOfWork, IActionBehavior innerBehaviour)
        {
            _unitOfWork = unitOfWork;
            _innerBehaviour = innerBehaviour;
        }

        //ctor with dependency on ISession and IActionBehavior 
        public void Invoke()
        {


            try
            {
                _innerBehaviour.Invoke();
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }

        }
        public void InvokePartial()
        {
            _innerBehaviour.InvokePartial();
        }

    }

    public class AuthorizationHandler : IAuthorizationFailureHandler
    {
        private readonly IOutputWriter _writer;
        private readonly IUrlRegistry _registry;

        public AuthorizationHandler(IOutputWriter writer, IUrlRegistry registry)
        {
            _writer = writer;
            _registry = registry;
        }

        public void Handle()
        {
            string url = _registry.UrlFor(new LoginInputModel());
            _writer.RedirectToUrl(url);
        }
    }
}