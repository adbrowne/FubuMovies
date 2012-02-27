using System.Collections.Generic;
using FubuMovies.Core;
using FubuMovies.Infrastructure;

namespace FubuMovies.Web.Timetable
{
    public class GetHandler
    {
        private readonly IUnitOfWork unifOfWork;

        public GetHandler(IUnitOfWork unifOfWork)
        {
            this.unifOfWork = unifOfWork;
        }

        public TimetableViewModel Execute(TimetableRequest request)
        {
            var sessions = unifOfWork.CurrentSession.CreateCriteria<MovieSession>().List<MovieSession>();
            return new TimetableViewModel
                       {
                           Sessions = sessions
                       };
        }
    }

    public class TimetableRequest
    {
    }

    public class TimetableViewModel
    {
        public IList<MovieSession> Sessions { get; set; }
    }
}