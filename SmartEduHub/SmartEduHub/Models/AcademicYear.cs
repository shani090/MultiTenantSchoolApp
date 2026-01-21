using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class AcademicYear
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsCurrent { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual College College { get; set; } = null!;

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();
}
