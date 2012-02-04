using FubuMovies.Core;
using FubuMovies.Infrastructure;

namespace FubuMovies.Timetable
{
    public class TimetableController
    {
        private readonly IUnitOfWork unifOfWork;

        public TimetableController(IUnitOfWork unifOfWork)
        {
            this.unifOfWork = unifOfWork;
        }

        public TimetableViewModel View(TimetableRequest request)
        {
            var session = unifOfWork.CurrentSession.Get<MovieSession>(1);
            return new TimetableViewModel();
        }
    }

    public class TimetableRequest
    {
    }

    public class TimetableViewModel
    {
    }
}