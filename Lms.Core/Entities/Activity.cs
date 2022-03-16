namespace Lms.Core.Entities;

public class Activity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ActivityType ActivityTypeId { get; set; } = default!;
    public int ModuleId { get; set; }
    public Document Document { get; set; } = default!;
}
