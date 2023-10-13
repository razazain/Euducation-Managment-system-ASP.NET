using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class ExamMark
{
    public int ExammarksId { get; set; }

    public int? TotalMarks { get; set; }

    public int? ObtainedMarks { get; set; }

    public int? CandidateId { get; set; }

    public int? ExamId { get; set; }

    public virtual Candidate? Candidate { get; set; }

    public virtual Exam? Exam { get; set; }
}
