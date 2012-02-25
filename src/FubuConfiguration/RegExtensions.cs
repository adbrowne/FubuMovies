using System.Linq;
using FubuCore;
using FubuMovies.Web.Api;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMovies.FubuConfiguration
{
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
}