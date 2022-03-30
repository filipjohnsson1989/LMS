using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lms.Core.Interfaces.Services;

public interface IActivityTypeService
{
    Task<IEnumerable<SelectListItem>> GetActivityTypes(int? selectedId);
}
