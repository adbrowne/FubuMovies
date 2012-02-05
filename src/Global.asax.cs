using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using FubuMovies.Infrastructure;
using FubuMovies.Timetable;
using FubuMovies.Login;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Security;
using FubuMVC.Core.Security.AntiForgery;
using FubuMVC.Core.UI.Extensibility;
using FubuMVC.Core.Urls;
using FubuMVC.Spark;
using FubuMVC.StructureMap;
using FubuMVC.Validation;
using FubuValidation;
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
                .ConstrainToHttpMethod(action => action.Method.Name.StartsWith("Index"), "GET")
                .ConstrainToHttpMethod(action => action.Method.Name.ToLower() == "get", "GET")
                .ConstrainToHttpMethod(action => action.Method.Name.ToLower() == "post", "POST");

            Actions.IncludeTypes(t => t.Name.EndsWith("Handler")).IgnoreMethodsDeclaredBy<AuthorizationHandler>();

            Policies.Add<AntiForgeryPolicy>();

            Policies.WrapBehaviorChainsWith<TransactionBehavior>();

            Policies.WrapBehaviorChainsWith<LoginBehaviour>().Ordering(a => a.MustBeBeforeAuthorization());

            this.UseSpark();

            Views
                .TryToAttachWithDefaultConventions()
                .TryToAttachViewsInPackages()
                .RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));

            //HtmlConvention<SampleHtmlConventions>();

            this.Validation(validation =>
                                {
                                    validation.Actions.Include(
                                        call =>
                                        call.HasInput &&
                                        call.InputType().GetInterfaces().Contains(typeof (IValidationModel)));

                                    validation
                                        .Failures
                                        .If(call => call.InputType() != null &&
                                                    call.InputType().GetInterfaces().Contains(typeof (IValidationModel)))
                                        .TransferBy<HandlerModelDescriptor>();
                                });

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

    public class HandlerModelDescriptor : IFubuContinuationModelDescriptor
    {
        private readonly BehaviorGraph _graph;

        public HandlerModelDescriptor(BehaviorGraph graph)
        {
            _graph = graph;
        }

        public Type DescribeModelFor(ValidationFailure context)
        {
            // Remember, behavior chains can be identified by the input model type
            // The IFubuContinuationModelDescriptor interface is used to describe the input model type of the chain
            // that we want to transfer to

            // we're going to query the BehaviorGraph to find the corresponding GET for the POST
            // obviously, we'd need to make this smarter but this is just a simple example
            var targetNamespace = context.Target.HandlerType.Namespace;
            var getCall = _graph
                .Behaviors
                .Where(chain => chain.FirstCall() != null && chain.FirstCall().HandlerType.Namespace == targetNamespace
                    && chain.Route.AllowedHttpMethods.Contains("GET"))
                .Select(chain => chain.FirstCall())
                .FirstOrDefault();

            if (getCall == null)
            {
                return null;
            }

            return getCall.InputType();
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
}