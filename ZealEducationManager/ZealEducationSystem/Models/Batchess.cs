using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class Batchess
{
    public int BatchId { get; set; }

    public string? BatchCode { get; set; }

    public string? BatchTime { get; set; }

    public string? BatchDays { get; set; }

    public DateTime? BatchStartDate { get; set; }

    public int? CourseId { get; set; }

    public int? FacultyId { get; set; }

    public string? BatchDuration { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public virtual Course? Course { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual Faculty? Faculty { get; set; }
}
