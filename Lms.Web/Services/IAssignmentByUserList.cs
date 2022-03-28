namespace Lms.Web.Services
{
    public interface IAssignmentByUserList
    {

        Task<IEnumerable<ApplicationUser>> GetStudentListAsync(int activityId);
    }
}