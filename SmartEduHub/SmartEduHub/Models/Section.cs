using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Section
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public int ClassId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual College College { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
