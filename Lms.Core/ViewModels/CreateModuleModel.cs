using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.ViewModels
{
    public class CreateModuleModel
    {
        //public int Id { get; set; }
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        [Remote(action: "VerifyStartdate", controller: "Modules",
            AdditionalFields = nameof(CourseId))]
        public DateTime StartDate { get; set; }
        [Remote(action: "VerifyEnddate", controller: "Modules",
           AdditionalFields = nameof(StartDate)) ]
        public DateTime EndDate { get; set; }
        public int CourseId { get; set; }
    }
}
