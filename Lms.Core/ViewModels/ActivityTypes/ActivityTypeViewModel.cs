using Lms.Core.ViewModels.Activities;

namespace Lms.Core.ViewModels.ActivityTypes;

public class ActivityTypeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<ActivityViewModel> Activities { get; set; } = default!;
}
