namespace Lms.Core.Entities;

public class ActivityType : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<Activity> Activities { get; set; } = default!;
}
