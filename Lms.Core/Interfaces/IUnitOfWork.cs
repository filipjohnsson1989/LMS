using Lms.Core.Entities;
namespace Lms.Core.Interfaces;

public interface IUnitOfWork
{
    ICourseRepository courseRepo { get; }
    IModuleRepository moduleRepo { get; }
    IActivityRepository activityRepo { get;}
    IRepository<ActivityType> ActivityTypeRepo { get;}
    IDocumentRepository documentRepo { get;}
    Task CompleteAsync();
    ICourseSelector courseSelector { get; }
}
