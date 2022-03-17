using Lms.Core.Interfaces;
using Lms.Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICourseRepository courseRepo { get; set; }
        public IModuleRepository moduleRepo { get;  set; }
        public IActivityRepository activityRepo { get; set; }
        public IActivityTypeRepository activityTypeRepo { get; set; }
        public IDocumentRepository documentRepo { get; set; }

        public ApplicationDbContext db;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.db = context;
            courseRepo = new CourseRepository(db);
            moduleRepo = new ModuleRepository(db);
            activityRepo = new ActivityRepository(db);
            documentRepo = new DocumentRepository(db);
            activityTypeRepo = new ActivityTypeRepository(db);
        }
        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
