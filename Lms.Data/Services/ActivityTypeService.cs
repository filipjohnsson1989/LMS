using AutoMapper;
using Lms.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Services;

public class ActivityTypeService : IActivityTypeService
{
    private readonly IUnitOfWork unitOfWork;

    public ActivityTypeService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SelectListItem>> GetActivityTypes(int? selectedId)
        => await unitOfWork.ActivityTypeRepo
        .GetAll()
        .Select(activityType
            => new SelectListItem()
            {
                Text = activityType.Name,
                Value = activityType.Id.ToString(),
                Selected = activityType.Id == selectedId
            })
        .ToListAsync();



}
