using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class College
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public string? LogoUrl { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public virtual ICollection<AcademicYear> AcademicYears { get; set; } = new List<AcademicYear>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
