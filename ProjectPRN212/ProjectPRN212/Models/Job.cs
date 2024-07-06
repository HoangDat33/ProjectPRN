using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class Job
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? AssignedBy { get; set; }

    public int? DepartmentId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? JobStatusId { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeletedById { get; set; }

    public virtual Employee? AssignedByNavigation { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; } = new List<EmployeeJob>();

    public virtual JobStatus? JobStatus { get; set; }
}
