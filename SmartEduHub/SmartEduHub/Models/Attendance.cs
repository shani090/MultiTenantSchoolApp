using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public int StudentId { get; set; }

    public DateOnly Date { get; set; }

    public string Status { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual College College { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
