using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class Enquiry
{
    public int EnqId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EnqEmail { get; set; }

    public string? EnqMessage { get; set; }
}
