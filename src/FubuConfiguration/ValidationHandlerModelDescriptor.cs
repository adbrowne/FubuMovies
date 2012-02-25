﻿using System;
using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Validation;

namespace FubuMovies.FubuConfiguration
{
    public class ValidationHandlerModelDescriptor : IFubuContinuationModelDescriptor
    {
        private readonly BehaviorGraph graph;

        public ValidationHandlerModelDescriptor(BehaviorGraph graph)
        {
            this.graph = graph;
        }

        public Type DescribeModelFor(ValidationFailure context)
        {
            // Remember, behavior chains can be identified by the input model type
            // The IFubuContinuationModelDescriptor interface is used to describe the input model type of the chain
            // that we want to transfer to

            // we're going to query the BehaviorGraph to find the corresponding GET for the POST
            // obviously, we'd need to make this smarter but this is just a simple example
            var targetNamespace = context.Target.HandlerType.Namespace;
            var getCall = graph
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
}