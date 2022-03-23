using Lms.Core.Entities;
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
        public IModuleRepository moduleRepo { get; set; }
        public IActivityRepository activityRepo { get; set; }

        private IRepository<ActivityType> activityTypeRepo = default!;
        public IRepository<ActivityType> ActivityTypeRepo
        {
            get
            {
                if (activityTypeRepo == null)
                {
                    this.activityTypeRepo = new ActivityTypeRepository(this.context);
                }

                return activityTypeRepo;
            }
        }

        private IRepository<Course> courseTypeRepoG = default!;
        public IRepository<Course> CourseRepoG
        {
            get
            {
                if (courseTypeRepoG == null)
                {
                    this.courseTypeRepoG = new CourseRepositoryG(this.context);
                }

                return courseTypeRepoG;
            }
        }

        private IRepository<Module> moduleTypeRepoG = default!;
        public IRepository<Module> ModuleRepoG
        {
            get
            {
                if (moduleTypeRepoG == null)
                {
                    this.moduleTypeRepoG = new ModuleRepositoryG(this.context);
                }

                return moduleTypeRepoG;
            }
        }
        public IDocumentRepository documentRepo { get; set; }


        public ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            courseRepo = new CourseRepository(this.context);
            moduleRepo = new ModuleRepository(this.context);
            activityRepo = new ActivityRepository(this.context);
            documentRepo = new DocumentRepository(this.context);
        }


        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
