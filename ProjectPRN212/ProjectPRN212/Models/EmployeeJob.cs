using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class EmployeeJob
{
    public int EmployeeJobId { get; set; }

    public int? EmployeeId { get; set; }

    public int? JobId { get; set; }

    public DateOnly? AssignmentDate { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeletedById { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Job? Job { get; set; }
}
