using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseName { get; set; }

    public string? CourseFees { get; set; }

    public string? CourseLength { get; set; }

    public string? CourseDescription { get; set; }

    public string? CourseThumbnail { get; set; }

    public virtual ICollection<Batchess> Batchesses { get; set; } = new List<Batchess>();

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
