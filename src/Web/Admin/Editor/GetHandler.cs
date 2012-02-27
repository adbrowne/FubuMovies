using FubuMVC.Core.Security;

namespace FubuMovies.Web.Admin.Editor
{
    [AllowRole("manager")]
    public class GetHandler
    {
        public EditTimetableViewModel Execute(EditorInputModel inputModel)
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