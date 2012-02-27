using System;
using System.Linq;
using FubuCore.Reflection;
using FubuCore;
using FubuMovies.Core;
using FubuMovies.Web.Api;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace FubuMovies.FubuConfiguration
{
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
            return call.HandlerType.Closes(typeof (ApiController<>));
            //return IsSubclassOfRawGeneric(typeof(ApiController<>), call.HandlerType);
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
}