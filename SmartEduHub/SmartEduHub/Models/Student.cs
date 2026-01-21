using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Student
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public Guid? UserId { get; set; }

    public string AdmissionNo { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int ClassId { get; set; }

    public int? SectionId { get; set; }

    public string? RollNo { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? FatherName { get; set; }

    public string? MotherName { get; set; }

    public string? Address { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Class Class { get; set; } = null!;

    public virtual College College { get; set; } = null!;

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual Section? Section { get; set; }

    public virtual User? User { get; set; }
}
