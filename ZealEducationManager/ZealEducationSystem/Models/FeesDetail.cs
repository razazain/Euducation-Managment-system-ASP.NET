using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class FeesDetail
{
    public int FeesId { get; set; }

    public string? CollectedAmount { get; set; }

    public int? CandidateId { get; set; }

    public string? FeeMonth { get; set; }

    public virtual Candidate? Candidate { get; set; }
}
