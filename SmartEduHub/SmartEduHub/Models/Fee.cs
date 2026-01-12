using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Fee
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public int StudentId { get; set; }

    public int AcademicYearId { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? DueAmount { get; set; }

    public DateOnly? DueDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual College College { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
