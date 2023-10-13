using System;
using System.Collections.Generic;

namespace ZealEducationSystem.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserPassword { get; set; }
}
