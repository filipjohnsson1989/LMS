using Lms.Core.Interfaces;

namespace Lms.Core.Entities;

public class Activity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ActivityType ActivityType { get; set; } = default!;
    public Module Module { get; set; } = default!;
    public int ModuleId { get; set; }
    public ICollection<Document> Documents { get; set; } = default!;
}
