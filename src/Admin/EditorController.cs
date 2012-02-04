using FubuMVC.Core.Security;

namespace FubuMovies.Admin
{
    [AllowRole("manager")]
    public class EditorController
    {
        public EditTimetableViewModel View(EditorInputModel inputModel)
        {
            return new EditTimetableViewModel();
        }
    }

    public class EditorInputModel
    {
    }

    public class EditTimetableViewModel
    {
    }
}