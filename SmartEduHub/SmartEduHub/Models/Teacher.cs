using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Subject { get; set; }

    public string? Qualification { get; set; }

    public int? ExperienceYears { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual College College { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
