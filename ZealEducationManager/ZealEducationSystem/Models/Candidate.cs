using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? CandidateEmail { get; set; }

    public int? CourseId { get; set; }

    public int? BatchId { get; set; }

    public string? CandidatePassword { get; set; }

    public string? CandidateImage { get; set; }

    public virtual Batchess? Batch { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<ExamMark> ExamMarks { get; set; } = new List<ExamMark>();

    public virtual ICollection<FeesDetail> FeesDetails { get; set; } = new List<FeesDetail>();
}
