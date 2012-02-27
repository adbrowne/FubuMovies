using System;
using System.Collections.Generic;
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