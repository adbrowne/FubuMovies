using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Security;
using System.Web.SessionState;
using FubuCore;
using FubuCore.Binding;
using FubuCore.Reflection;
using FubuMovies.Core;
using FubuMovies.Infrastructure;
using FubuMovies.Login;
using FubuMovies.Timetable;
using FubuMovies.Web;
using FubuMovies.Web.Api;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Registration.Routes;
using FubuMVC.Core.Security;
using FubuMVC.Core.Security.AntiForgery;
using FubuMVC.Core.UI;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Core.UI.Extensibility;
using FubuMVC.Core.Urls;
using FubuMVC.Spark;
using FubuMVC.StructureMap;
using FubuMVC.Validation;
using FubuValidation;
using HtmlTags;
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
            IncludeDiagnostics(true);
            Applies
                .ToThisAssembly();

            Actions
                .IncludeClassesSuffixedWithController();

            Actions.IncludeType<ApiController<Movie>>();
            Actions.IncludeType<ApiController<MovieSession>>();

            ApplyHandlerConventions();

            Routes
                .HomeIs<TimetableRequest>()
                .IgnoreControllerNamespaceEntirely()
                .IgnoreMethodSuffix("Command")
                .IgnoreMethodSuffix("Query")
                .UrlPolicy(new MyUrlPolicy())
                .ConstrainToHttpMethod(action => action.Method.Name.EndsWith("Command"), "POST")
                .ConstrainToHttpMethod(action => action.Method.Name.StartsWith("Index"), "GET")
                .ConstrainToHttpMethod(action => action.Method.Name.ToLower() == "get", "GET")
                .ConstrainToHttpMethod(action => action.Method.Name.ToLower() == "post", "POST");

            ApplyHandlerConventions(typeof(HandlersMarker));

            //Models
            //    .BindModelsWith<EntityModelBinder>();

            Actions.IncludeTypes(t => t.Name.EndsWith("Handler")).IgnoreMethodsDeclaredBy<AuthorizationHandler>();

            Policies.Add<AntiForgeryPolicy>();

            Policies.WrapBehaviorChainsWith<TransactionBehavior>();

            Policies.WrapBehaviorChainsWith<LoginBehaviour>().Ordering(a => a.MustBeBeforeAuthorization());

            Configure(graph =>
                RegExtensions.ApplyRedirectOnAddAndUpdate(graph)
);

            //Policies.ConditionallyWrapBehaviorChainsWith<RedirectOnAddAndUpdateBehavior>(
            //    x => x.OutputType().Closes(typeof(ViewModel<>)));
            this.UseSpark();

            Views
                .TryToAttachWithDefaultConventions()
                .TryToAttachViewsInPackages()
                .RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));

            HtmlConvention<DefaultHtmlConventions>();
            HtmlConvention<MyPasswordConvention>();
            HtmlConvention<MyLoginFormConvention>();

            this.Validation(validation =>
                                {
                                    validation.Actions.Include(
                                        call =>
                                        call.HasInput &&
                                        call.InputType().GetInterfaces().Contains(typeof(IValidationModel)));

                                    validation
                                        .Failures
                                        .If(call => call.InputType() != null &&
                                                    call.InputType().GetInterfaces().Contains(typeof(IValidationModel)))
                                        .TransferBy<HandlerModelDescriptor>();
                                });

            Services(s =>
            {
                //s.FillType<IExceptionHandler, AsyncExceptionHandler>();
                s.ReplaceService<IUrlTemplatePattern, JQueryUrlTemplate>();
                s.AddService<IAuthorizationFailureHandler, AuthorizationHandler>();
                s.AddService<IElementNamingConvention, DotNotationElementNamingConvention>();
            });

            this.Extensions()
                .For<Timetable.TimetableViewModel>("extension-placeholder", x => "<p>Rendered from content extension.</p>");
        }
    }

    public static class RegExtensions
    {
        public static void ApplyRedirectOnAddAndUpdate(this BehaviorGraph graph)
        {
            foreach (var chain in graph.Behaviors)
            {
                var actionOutputType = chain.ActionOutputType();

                if (actionOutputType.Closes(typeof(ViewModel<>)))
                {
                    var outputNode = new RedirectOutputNode(actionOutputType);
                    var action = chain.Last(x => x is ActionCall);
                    action.AddAfter(outputNode);
                }    
            }
        }
    }

    public class RedirectOutputNode : BehaviorNode
    {
        private readonly Type inputType;

        public RedirectOutputNode(Type inputType)
        {
            this.inputType = inputType;
        }

        protected override ObjectDef buildObjectDef()
        {
            var objectDef = new ObjectDef(typeof(RedirectOnAddAndUpdateBehavior<>).MakeGenericType(inputType.GetGenericArguments().First()));
            return objectDef;
        }

        public override BehaviorCategory Category
        {
            get { return BehaviorCategory.Conditional; }
        }
    }

    public class MyLoginFormConvention : HtmlConventionRegistry
    {
        public MyLoginFormConvention()
        {
            Editors.If(x => x.ModelType.Equals(typeof(LoginViewModel))).AddClass("login");
        }
    }

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

    public class EntityModelBinder : IModelBinder
    {
        public bool Matches(Type type)
        {
            return type.CanBeCastTo<IEntity>();
        }

        public void Bind(Type type, object instance, IBindingContext context)
        {
            throw new NotImplementedException();
        }

        public object Bind(Type type, IBindingContext context)
        {
            var entity = context.ValueAs(type, "Id");

            if (entity != null) return entity;

            if (type.IsConcrete())
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }

    public class MyUrlPolicy : IUrlPolicy
    {
        //from: http://stackoverflow.com/questions/457676/c-sharp-reflection-check-if-a-class-is-derived-from-a-generic-class
        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        static Type GetGenericParameter(Type generic)
        {
            return generic.GetGenericArguments()[0];
        }

        public bool Matches(ActionCall call, IConfigurationObserver log)
        {
            return IsSubclassOfRawGeneric(typeof(ApiController<>), call.HandlerType);
        }

        public IRouteDefinition Build(ActionCall call)
        {
            var entityType = GetGenericParameter(call.HandlerType);
            var pluralName = GetPluralName(entityType);
            if (call.Method.Name == "New")
            {
                var routeDefinition = call.ToRouteDefinition();
                routeDefinition.Append("api");
                routeDefinition.Append(pluralName);
                routeDefinition.Append("new");
                routeDefinition.AddHttpMethodConstraint("GET");
                return routeDefinition;
            }
            else if (call.Method.Name == "List")
            {
                var routeDefinition = call.ToRouteDefinition();
                routeDefinition.Append("api");
                routeDefinition.Append(pluralName);
                routeDefinition.AddHttpMethodConstraint("GET");
                return routeDefinition;
            }
            else if (call.Method.Name == "Add")
            {
                var routeDefinition = call.ToRouteDefinition();
                routeDefinition.Append("api");
                routeDefinition.Append(pluralName);
                routeDefinition.AddHttpMethodConstraint("POST");
                return routeDefinition;
            }

            else if (call.Method.Name == "Get")
            {
                var routeDefinition = call.ToRouteDefinition();
                routeDefinition.Append("api");
                routeDefinition.Append(pluralName);

                routeDefinition.Input.AddRouteInput(new RouteParameter(new SingleProperty(call.InputType().GetProperty("Id"))), true);
                routeDefinition.AddHttpMethodConstraint("GET");
                return routeDefinition;
            }

            else if (call.Method.Name == "Update")
            {
                var routeDefinition = call.ToRouteDefinition();
                routeDefinition.Append("api");
                routeDefinition.Append(pluralName);

                routeDefinition.Input.AddRouteInput(new RouteParameter(new SingleProperty(call.InputType().GetProperty("Id"))), true);
                routeDefinition.AddHttpMethodConstraint("POST");
                return routeDefinition;
            }

            throw new InvalidOperationException("Unknown method: " + call.Method.Name);
        }

        private static string GetPluralName(Type entityType)
        {
            foreach (var attrib in entityType.GetCustomAttributes(false).OfType<PluralAttribute>())
            {
                return attrib.Plural;
            }
            return entityType.Name;
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