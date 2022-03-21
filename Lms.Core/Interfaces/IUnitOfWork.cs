using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepo { get; }
        ICourseRepository courseRepo { get; }
        IModuleRepository moduleRepo { get; }
        IActivityRepository activityRepo { get;}
        IActivityTypeRepository activityTypeRepo { get;}
        IDocumentRepository documentRepo { get;}
        Task CompleteAsync();
    }
}
