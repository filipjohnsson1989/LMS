﻿namespace Lms.Core.Dtos.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime StartDate { get; set; }

}