using System;
using System.Collections.Generic;

namespace SmartEduHub.Models;

public partial class Result
{
    public int Id { get; set; }

    public int CollegeId { get; set; }

    public int StudentId { get; set; }

    public int ExamId { get; set; }

    public string Subject { get; set; } = null!;

    public decimal MarksObtained { get; set; }

    public decimal TotalMarks { get; set; }

    public string? Grade { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual College College { get; set; } = null!;

    public virtual Exam Exam { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
