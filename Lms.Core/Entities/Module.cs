﻿namespace Lms.Core.Entities;

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<Activity> Activities { get; set; } = default!;
    public ICollection<Document> Documents { get; set; } = default!;

}
