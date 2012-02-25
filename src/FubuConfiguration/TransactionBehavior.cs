using FubuMovies.Infrastructure;
using FubuMVC.Core.Behaviors;

namespace FubuMovies.FubuConfiguration
{
    class TransactionBehavior : IActionBehavior
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IActionBehavior innerBehaviour;

        public TransactionBehavior(IUnitOfWork unitOfWork, IActionBehavior innerBehaviour)
        {
            this.unitOfWork = unitOfWork;
            this.innerBehaviour = innerBehaviour;
        }

        //ctor with dependency on ISession and IActionBehavior 
        public void Invoke()
        {


            try
            {
                innerBehaviour.Invoke();
                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

        }
        public void InvokePartial()
        {
            innerBehaviour.InvokePartial();
        }

    }
}