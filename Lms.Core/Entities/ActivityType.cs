﻿namespace Lms.Core.Entities;

public class ActivityType
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<ActivityType> ActivityTypes { get; set; } = default!;
}
