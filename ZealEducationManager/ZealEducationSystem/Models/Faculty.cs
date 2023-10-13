using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class Faculty
{
    public int FacultyId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FacultyEmail { get; set; }

    public string? FacultyPassword { get; set; }

    public virtual ICollection<Batchess> Batchesses { get; set; } = new List<Batchess>();
}
