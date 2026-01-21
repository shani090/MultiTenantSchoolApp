using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Class
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual College College { get; set; } = null!;

    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
