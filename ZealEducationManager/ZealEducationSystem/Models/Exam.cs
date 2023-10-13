using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class Exam
{
    public int ExamId { get; set; }

    public DateTime? ExamDate { get; set; }

    public TimeSpan? ExamTime { get; set; }

    public int? ExamtypeId { get; set; }

    public int? CourseId { get; set; }

    public int? BatchId { get; set; }

    public string? ExaxmName { get; set; }

    public virtual Batchess? Batch { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<ExamMark> ExamMarks { get; set; } = new List<ExamMark>();

    public virtual ExamType? Examtype { get; set; }
}
