using System;
using System.Reflection;
using NUnit.Framework;
using StoryQ;

namespace FubuMovies.Tests.Controller
{
    [TestFixture]
    public class TimetableControllerTests
    {
        [Test]
        public void TimetableRequest()
        {
            new Story("Timetable View")
            .InOrderTo("View the current timetable")
            .AsA("Consumer")
            .IWant("The timetable to start today")
            .WithScenario("View timetable")
                .Given(IHaveHaveNotSpecifiedAnyOtherDate)
                .When(IViewThePage)
                .Then(TheFirstDayShouldBeToday)
                .ExecuteWithReport(MethodBase.GetCurrentMethod());
        }

        private void TheFirstDayShouldBeToday()
        {
            
        }

        private void IViewThePage()
        {
            
        }

        private void IHaveHaveNotSpecifiedAnyOtherDate()
        {
            
        }
    }
}