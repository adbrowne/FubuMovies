using FubuMVC.Core.Security;

namespace FubuMovies.Web.Admin
{
    [AllowRole("manager")]
    public class EditorController
    {
        public EditTimetableViewModel get(EditorInputModel inputModel)
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