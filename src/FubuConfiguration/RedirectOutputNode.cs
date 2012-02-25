using System;
using System.Linq;
using FubuMovies.Infrastructure;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;

namespace FubuMovies.FubuConfiguration
{
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
}