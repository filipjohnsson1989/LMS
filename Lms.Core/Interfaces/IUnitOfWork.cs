using Lms.Core.Entities;
namespace Lms.Core.Interfaces;

public interface IUnitOfWork
{
    ICourseRepository courseRepo { get; }
    IRepository<Course> CourseRepoG { get; }
    IModuleRepository moduleRepo { get; }
    IRepository<Module> ModuleRepoG { get; }
    IActivityRepository activityRepo { get;}
    IRepository<ActivityType> ActivityTypeRepo { get;}
    IDocumentRepository documentRepo { get;}
    Task CompleteAsync();
}
