using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class SignupDetail
{
    public int UserId { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPassword { get; set; }

    public string? UserRole { get; set; }
}
