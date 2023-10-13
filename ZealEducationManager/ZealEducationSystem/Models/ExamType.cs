using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class ExamType
{
    public int ExamtypeId { get; set; }

    public string? ExamType1 { get; set; }

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
