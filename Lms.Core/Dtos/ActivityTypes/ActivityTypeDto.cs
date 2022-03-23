using Lms.Core.Dtos.Activities;
namespace Lms.Core.Dtos.ActivityTypes;

public class ActivityTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<ActivityDto> Activities { get; set; } = default!;
}
